using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Routing;
using PipServices3.Commons.Config;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Refer;
using PipServices3.Commons.Run;
using PipServices3.Commons.Validate;
using PipServices3.Components.Count;
using PipServices3.Components.Log;
using PipServices3.Rpc.Connect;

namespace PipServices3.Grpc.Services
{
    /// <summary>
    /// Used for creating HTTP endpoints. An endpoint is a URL, at which a given service can be accessed by a client. 
    /// 
    /// ### Configuration parameters ###
    /// 
    /// Parameters to pass to the <c>Configure()</c> method for component configuration:
    /// 
    /// connection(s) - the connection resolver's connections;
    /// - "connection.discovery_key" - the key to use for connection resolving in a discovery service;
    /// - "connection.protocol" - the connection's protocol;
    /// - "connection.host" - the target host;
    /// - "connection.port" - the target port;
    /// - "connection.uri" - the target URI.
    ///
    /// credential - the HTTPS credentials:
    /// - "credential.ssl_key_file" - the SSL private key in PEM
    /// - "credential.ssl_crt_file" - the SSL certificate in PEM
    /// - "credential.ssl_ca_file" - the certificate authorities (root cerfiticates) in PEM
    /// 
    /// ### References ###
    /// 
    /// A logger, counters, and a connection resolver can be referenced by passing the
    /// following references to the object's setReferences() method:
    /// 
    /// - logger: <code>"*:logger:*:*:1.0"</code>;
    /// - counters: <code>"*:counters:*:*:1.0"</code>;
    /// - discovery: <code>"*:discovery:*:*:1.0"</code> (for the connection resolver).
    /// </summary>
    /// <example>
    /// <code>
    /// public MyMethod(string correlationId, ConfigParams _config, IReferences _references) 
    /// {
    ///     var endpoint = new HttpEndpoint();
    ///     if (this._config)
    ///         endpoint.Configure(this._config);
    ///     if (this._references)
    ///         endpoint.SetReferences(this._references);
    ///     ...
    ///     this._endpoint.Open(correlationId);
    ///     ...
    /// }
    /// </code>
    /// </example>
    public class GrpcEndpoint : IOpenable, IConfigurable, IReferenceable
    {
        private static readonly ConfigParams _defaultConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "0.0.0.0",
            "connection.port", 3000,
            "credential.ssl_key_file", null,
            "credential.ssl_crt_file", null,
            "credential.ssl_ca_file", null,
            "options.maintenance_enabled", false,
            "options.request_max_size", 1024 * 1024,
            "options.file_max_size", 200 * 1024 * 1024,
            "options.connect_timeout", 60000,
            "options.debug", true,
            "options.response_compression", false
        );

        protected HttpConnectionResolver _connectionResolver = new HttpConnectionResolver();
        protected CompositeLogger _logger = new CompositeLogger();
        protected CompositeCounters _counters = new CompositeCounters();
        protected DependencyResolver _dependencyResolver = new DependencyResolver(_defaultConfig);

        private bool _maintenanceEnabled;
        private long _fileMaxSize = 200 * 1024 * 1024;
        private bool _responseCompression = false;

        protected Server _server;
        protected RouteBuilder _routeBuilder;
        protected string _address;

        private IList<IRegisterable> _registrations = new List<IRegisterable>();
        private List<Interceptor> _interceptors = new List<Interceptor>();
        private Dictionary<string, Func<string, Parameters, Task<object>>> _commandableMethods;

        /// <summary>
        /// Sets references to this endpoint's logger, counters, and connection resolver.
        /// 
        /// __References:__ - logger: <code>"\*:logger:\*:\*:1.0"</code> - counters:
        /// <code>"\*:counters:\*:\*:1.0"</code> - discovery:
        /// <code>"\*:discovery:\*:\*:1.0"</code> (for the connection resolver)
        /// </summary>
        /// <param name="references">an IReferences object, containing references to a logger, 
        /// counters, and a connection resolver.</param>
        public virtual void SetReferences(IReferences references)
        {
            _logger.SetReferences(references);
            _counters.SetReferences(references);
            _dependencyResolver.SetReferences(references);
            _connectionResolver.SetReferences(references);
        }

        /// <summary>
        /// Configures this HttpEndpoint using the given configuration parameters.
        /// 
        /// __Configuration parameters:__ - __connection(s)__ - the connection resolver's
        /// connections; - "connection.discovery_key" - the key to use for connection
        /// resolving in a discovery service; - "connection.protocol" - the connection's
        /// protocol; - "connection.host" - the target host; - "connection.port" - the
        /// target port; - "connection.uri" - the target URI.
        /// </summary>
        /// <param name="config">configuration parameters, containing a "connection(s)" section.</param>
        /// See <see cref="ConfigParams"/>
        public virtual void Configure(ConfigParams config)
        {
            config = config.SetDefaults(_defaultConfig);
            _dependencyResolver.Configure(config);
            _connectionResolver.Configure(config);

            _maintenanceEnabled = config.GetAsBooleanWithDefault("options.maintenance_enabled", _maintenanceEnabled);
            _fileMaxSize = config.GetAsLongWithDefault("options.file_max_size", _fileMaxSize);
            _responseCompression = config.GetAsBooleanWithDefault("options.response_compression", _responseCompression);
        }

        /// <summary>
        /// Adds instrumentation to log calls and measure call time. It returns a Timing 
        /// object that is used to end the time measurement.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="name">a method name.</param>
        /// <returns>Timing object to end the time measurement.</returns>
        protected Timing Instrument(string correlationId, string name)
        {
            _logger.Trace(correlationId, "Executing {0} method", name);
            return _counters.BeginTiming(name + ".exec_time");
        }

        /// <summary>
        /// Checks if the component is opened.
        /// </summary>
        /// <returns>whether or not this endpoint is open with an actively listening gRPC server.</returns>
        public virtual bool IsOpen()
        {
            return _server != null;
        }

        /// <summary>
        /// Opens a connection using the parameters resolved by the referenced connection
        /// resolver and creates a gRPC server(service) using the set options and parameters.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        public virtual async Task OpenAsync(string correlationId)
        {
            if (IsOpen()) return;

            var connection = await _connectionResolver.ResolveAsync(correlationId);
            var credential = connection.GetSection("credential");

            var protocol = connection.Protocol;
            var host = connection.Host;
            var port = connection.Port;
            _address = protocol + "://" + host + ":" + port;

            try
            {
                _server = new Server
                {
                    Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
                };

                _logger.Info(correlationId, "Opened gRPC service at {0}", _address);
                
                PerformRegistrations();
                _server.Start();

                await Task.Delay(0);
            }
            catch (Exception ex)
            {
                if (_server != null)
                {
                    //_server.Dispose();
                    _server = null;
                }

                throw new ConnectionException(correlationId, "CANNOT_CONNECT", "Opening gRPC service failed")
                    .WithCause(ex).WithDetails("url", _address);
            }
        }

        /// <summary>
        /// Closes this endpoint and the gRPC server (service) that was opened earlier.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        public virtual Task CloseAsync(string correlationId)
        {
            if (_server != null)
            {
                _commandableMethods = null;

                // Eat exceptions
                try
                {
                    _server.ShutdownAsync();
                    _logger.Info(correlationId, "Closed gRPC service at {0}", _address);
                }
                catch (Exception ex)
                {
                    _logger.Warn(correlationId, "Failed while closing gRPC service: {0}", ex);
                }

                _server = null;
                _address = null;
            }

            return Task.Delay(0);
        }

        private void PerformRegistrations()
        {
            // Delegate registering routes
            foreach (var registration in _registrations)
            {
                registration.Register();
            }

            RegisterCommandableService();
        }

        private void RegisterCommandableService()
        {
            if (_commandableMethods == null) return;

            RegisterService(Commandable.Commandable.BindService(new CommandableGrpcServerService(_commandableMethods)));
        }

        /// <summary>
        /// Registers a commandable method in this objects GRPC server (service) by the given name.,
        /// </summary>
        /// <param name="method">the GRPC method name.</param>
        /// <param name="schema">the schema to use for parameter validation.</param>
        /// <param name="action">the action to perform at the given route.</param>
        public void RegisterCommandableMethod(string method, Schema schema, Func<string, Parameters, Task<object>> action)
        {
            _commandableMethods = _commandableMethods ?? new Dictionary<string, Func<string, Parameters, Task<object>>>();
            _commandableMethods[method] = action;
        }
        
        public void RegisterService(ServerServiceDefinition serverServiceDefinition)
        {
            _server.Services.Add(serverServiceDefinition);
        }

        /// <summary>
        /// Registers a registerable object for dynamic endpoint discovery.
        /// </summary>
        /// <param name="registration">the registration to add.</param>
        public void Register(IRegisterable registration)
        {
            _registrations.Add(registration);
        }

        /// <summary>
        /// Unregisters a registerable object, so that it is no longer used in dynamic endpoint discovery.
        /// </summary>
        /// <param name="registration">the registration to remove.</param>
        public void Unregister(IRegisterable registration)
        {
            _registrations.Remove(registration);
        }
    }
}