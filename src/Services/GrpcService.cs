using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PipServices3.Commons.Config;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Refer;
using PipServices3.Commons.Run;
using PipServices3.Commons.Validate;
using PipServices3.Components.Count;
using PipServices3.Components.Log;

namespace PipServices3.Grpc.Services
{
    /// <summary>
    /// Abstract service that receives remove calls via HTTP/REST protocol.
    /// 
    /// ### Configuration parameters ###
    /// 
    /// - base_route:              base route for remote URI
    /// 
    /// dependencies:
    /// - endpoint:              override for HTTP Endpoint dependency
    /// - controller:            override for Controller dependency
    /// 
    /// connection(s):
    /// - discovery_key:         (optional) a key to retrieve the connection from <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_connect_1_1_i_discovery.html">IDiscovery</a>
    /// - protocol:              connection protocol: http or https
    /// - host:                  host name or IP address
    /// - port:                  port number
    /// - uri:                   resource URI or connection string with all parameters in it
    /// 
    /// ### References ###
    /// 
    /// - *:logger:*:*:1.0          (optional) <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_log_1_1_i_logger.html">ILogger</a> components to pass log messages
    /// - *:counters:*:*:1.0        (optional) <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_count_1_1_i_counters.html">ICounters</a> components to pass collected measurements
    /// - *:discovery:*:*:1.0       (optional) <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_connect_1_1_i_discovery.html">IDiscovery</a> services to resolve connection
    /// - *:endpoint:http:*:1.0     (optional) <a href="https://pip-services3-dotnet.github.io/pip-services3-rpc-dotnet/class_pip_services_1_1_rpc_1_1_services_1_1_http_endpoint.html">HttpEndpoint</a/a> reference
    /// </summary>
    /// <example>
    /// <code>
    /// class MyRestService: RestService 
    /// {
    ///     private IMyController _controller;
    ///     ...
    ///     public MyRestService()
    ///     {
    ///         base();
    ///         this._dependencyResolver.put(
    ///         "controller", new Descriptor("mygroup", "controller", "*", "*", "1.0"));
    ///     }
    ///     
    ///     public void SetReferences(IReferences references)
    ///     {
    ///         base.SetReferences(references);
    ///         this._controller = this._dependencyResolver.getRequired<IMyController>("controller");
    ///     }
    ///     
    ///     public void register()
    ///     {
    ///         ...
    ///     }
    /// }
    /// 
    /// var service = new MyRestService();
    /// service.Configure(ConfigParams.fromTuples(
    /// "connection.protocol", "http",
    /// "connection.host", "localhost",
    /// "connection.port", 8080 ));
    /// 
    /// service.SetReferences(References.fromTuples(
    /// new Descriptor("mygroup","controller","default","default","1.0"), controller ));
    /// 
    /// service.Open("123");
    /// Console.Out.WriteLine("The REST service is running on port 8080");
    /// </code>
    /// </example>
    public abstract class GrpcService : IOpenable, IConfigurable, IReferenceable, IUnreferenceable, IRegisterable
    {
        private static readonly ConfigParams _defaultConfig = ConfigParams.FromTuples(
            "dependencies.endpoint", "*:endpoint:grpc:*:1.0"
        );

        /// <summary>
        /// The HTTP endpoint that exposes this service.
        /// </summary>
        protected GrpcEndpoint _endpoint;
        /// <summary>
        /// The logger.
        /// </summary>
        protected CompositeLogger _logger = new CompositeLogger();
        /// <summary>
        /// The performance counters.
        /// </summary>
        protected CompositeCounters _counters = new CompositeCounters();
        /// <summary>
        /// The dependency resolver.
        /// </summary>
        protected DependencyResolver _dependencyResolver = new DependencyResolver(_defaultConfig);

        private ConfigParams _config;
        private IReferences _references;
        private bool _localEndpoint;
        private List<Interceptor> _interceptors = new List<Interceptor>();
        private bool _opened;
        protected string _serviceName;
        private readonly ServerServiceDefinition.Builder _builder = ServerServiceDefinition.CreateBuilder();
        private readonly Dictionary<Type, object> _messageParsers = new Dictionary<Type, object>();

        public GrpcService(string serviceName)
        {
            _serviceName = serviceName;
        }
        
        /// <summary>
        /// Configures component by passing configuration parameters.
        /// </summary>
        /// <param name="config">configuration parameters to be set.</param>
        public virtual void Configure(ConfigParams config)
        {
            _config = config.SetDefaults(_defaultConfig);
            _dependencyResolver.Configure(config);

            _serviceName = config.GetAsStringWithDefault("service_name", _serviceName);
        }

        /// <summary>
        /// Sets references to dependent components.
        /// </summary>
        /// <param name="references">references to locate the component dependencies.</param>
        public virtual void SetReferences(IReferences references)
        {
            _references = references;

            _logger.SetReferences(references);
            _counters.SetReferences(references);
            _dependencyResolver.SetReferences(references);

            // Get endpoint
            _endpoint = _dependencyResolver.GetOneOptional("endpoint") as GrpcEndpoint;
            _localEndpoint = _endpoint == null;

            // Or create a local one
            if (_endpoint == null)
                _endpoint = CreateLocalEndpoint();

            // Add registration callback to the endpoint
            _endpoint.Register(this);
        }

        /// <summary>
        /// Unsets (clears) previously set references to dependent components.
        /// </summary>
        public virtual void UnsetReferences()
        {
            // Remove registration callback from endpoint
            if (_endpoint != null)
            {
                _endpoint.Unregister(this);
                _endpoint = null;
            }
        }

        private GrpcEndpoint CreateLocalEndpoint()
        {
            var endpoint = new GrpcEndpoint();

            if (_config != null)
                endpoint.Configure(_config);

            if (_references != null)
                endpoint.SetReferences(_references);

            return endpoint;
        }

        /// <summary>
        /// Adds instrumentation to log calls and measure call time. It returns a Timing
        /// object that is used to end the time measurement.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="methodName">a method name.</param>
        /// <returns>Timing object to end the time measurement.</returns>
        protected Timing Instrument(string correlationId, string methodName)
        {
            _logger.Trace(correlationId, "Executing {0} method", methodName);
            _counters.IncrementOne(methodName + ".exec_count");
            return _counters.BeginTiming(methodName + ".exec_time");
        }

        /// <summary>
        /// Adds instrumentation to error handling.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="methodName">a method name.</param>
        /// <param name="ex">Error that occured during the method call</param>
        /// <param name="rethrow">True to throw the exception</param>
        protected void InstrumentError(string correlationId, string methodName, Exception ex, bool rethrow = false)
        {
            _logger.Error(correlationId, ex, "Failed to execute {0} method", methodName);
            _counters.IncrementOne(methodName + ".exec_errors");

            if (rethrow)
                throw ex;
        }

        /// <summary>
        /// Checks if the component is opened.
        /// </summary>
        /// <returns>true if the component has been opened and false otherwise.</returns>
        public bool IsOpen()
        {
            return _opened;
        }

        /// <summary>
        /// Opens the component.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <returns></returns>
        public async virtual Task OpenAsync(string correlationId)
        {
            if (IsOpen()) return;

            if (_endpoint == null)
            {
                _endpoint = CreateLocalEndpoint();
                _endpoint.Register(this);
                _localEndpoint = true;
            }

            if (_localEndpoint)
            {
                await _endpoint.OpenAsync(correlationId).ContinueWith(task =>
                {
                    _opened = task.Exception == null;
                });
            }
            else
            {
                _opened = true;
            }
        }

        /// <summary>
        /// Closes component and frees used resources.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <returns></returns>
        public virtual Task CloseAsync(string correlationId)
        {
            if (IsOpen())
            {
                if (_endpoint == null)
                {
                    throw new InvalidStateException(correlationId, "NO_ENDPOINT", "gRPC endpoint is missing");
                }

                if (_localEndpoint)
                {
                    _endpoint.CloseAsync(correlationId);
                }

                _opened = false;
            }

            return Task.Delay(0);
        }

        /// <summary>
        /// Registers a method in GRPC service.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        protected void RegisterMethod<TRequest, TResponse>(string name, UnaryServerMethod<TRequest, TResponse> handler)
            where TRequest : class, IMessage<TRequest>, new()
            where TResponse : class, IMessage<TResponse>, new()
        {
            var requestParser = GetOrCreateMessageParser<TRequest>();
            var responseParser = GetOrCreateMessageParser<TResponse>();

            var method = new Method<TRequest, TResponse>(
             MethodType.Unary,
              _serviceName,
              name,
              Marshallers.Create((arg) => arg != null ? MessageExtensions.ToByteArray(arg) : Array.Empty<byte>(), requestParser.ParseFrom),
              Marshallers.Create((arg) => arg != null ? MessageExtensions.ToByteArray(arg) : Array.Empty<byte>(), responseParser.ParseFrom));

            _builder.AddMethod(method, handler);
        }

        private MessageParser<T> GetOrCreateMessageParser<T>()
          where T : class, IMessage<T>, new()
        {
            if (_messageParsers.TryGetValue(typeof(T), out object o_parser))
                return o_parser as MessageParser<T>;

            MessageParser<T> parser = new MessageParser<T>(() => new T());
            _messageParsers.Add(typeof(T), parser);

            return parser;
        }

        /// <summary>
        /// Registers all service routes in gRPC endpoint.
        /// 
        /// This method is called by the service and must be overriden
        /// in child classes.
        /// </summary>
        protected virtual void OnRegister()
        { 
        }

        public void Register()
        {
            OnRegister();

            var serviceDefinitions = _builder
                .Build()
                .Intercept(_interceptors.ToArray());

            _endpoint.RegisterService(serviceDefinitions);
        }
    }
}
