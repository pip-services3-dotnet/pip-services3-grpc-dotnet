using Grpc.Core;
using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using PipServices3.Commons.Run;
using PipServices3.Components.Count;
using PipServices3.Components.Log;
using PipServices3.Rpc.Connect;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PipServices3.Grpc.Clients
{
	public class GrpcClient<T> : IOpenable, IConfigurable, IReferenceable
		where T : ClientBase<T>
	{
		private static readonly ConfigParams _defaultConfig = ConfigParams.FromTuples(
			"connection.protocol", "http",
			"connection.host", "0.0.0.0",
			"connection.port", 3000,

			"options.request_max_size", 1024*1024,
			"options.connect_timeout", 10000,
			"options.timeout", 10000,
			"options.retries", 3,
			"options.debug", true
		);

		/// <summary>
		/// The GRPC client
		/// </summary>
		protected T _client;

		protected Channel _channel;

		/// <summary>
		/// The connection resolver.
		/// </summary>
		protected HttpConnectionResolver _connectionResolver = new HttpConnectionResolver();
		/// <summary>
		/// The logger.
		/// </summary>
		protected CompositeLogger _logger = new CompositeLogger();
		/// <summary>
		/// The performance counters.
		/// </summary>
		protected CompositeCounters _counters = new CompositeCounters();
		/// <summary>
		/// The configuration options.
		/// </summary>
		protected ConfigParams _options = new ConfigParams();

		/// <summary>
		/// he connection timeout in milliseconds.
		/// </summary>
		protected int _connectTimeout = 10000;

		/// <summary>
		/// Sets references to dependent components.
		/// </summary>
		/// <param name="references">references to locate the component dependencies.</param>
		public virtual void SetReferences(IReferences references)
		{
			_logger.SetReferences(references);
			_counters.SetReferences(references);
			_connectionResolver.SetReferences(references);
		}

		/// <summary>
		/// Configures component by passing configuration parameters.
		/// </summary>
		/// <param name="config">configuration parameters to be set.</param>
		public virtual void Configure(ConfigParams config)
		{
			config = config.SetDefaults(_defaultConfig);
			_connectionResolver.Configure(config);
			_options = _options.Override(config.GetSection("options"));

			_connectTimeout = config.GetAsIntegerWithDefault("options.connect_timeout", _connectTimeout);
			//_timeout = config.GetAsIntegerWithDefault("options.timeout", _timeout);
		}

		/// <summary>
		/// Checks if the component is opened.
		/// </summary>
		/// <returns>true if the component has been opened and false otherwise.</returns>
		public virtual bool IsOpen()
		{
			return _client != null;
		}

		/// <summary>
		/// Opens the component.
		/// </summary>
		/// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
		public async virtual Task OpenAsync(string correlationId)
		{
			var connection = await _connectionResolver.ResolveAsync(correlationId);

			var uri = connection.Uri;
			var host = connection.Host;
			var port = connection.Port;

			_channel = new Channel(string.Format("{0}:{1}", host, port), ChannelCredentials.Insecure);

			_client = (T)Activator.CreateInstance(typeof(T), _channel);
		}

		/// <summary>
		/// Closes component and frees used resources.
		/// </summary>
		/// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
		public async virtual Task CloseAsync(string correlationId)
		{
			await _channel.ShutdownAsync();
		}

		/// <summary>
		/// Adds instrumentation to log calls and measure call time. It returns a Timing
		/// object that is used to end the time measurement.
		/// </summary>
		/// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
		/// <param name="methodName">a method name.</param>
		/// <returns>Timing object to end the time measurement.</returns>
		protected Timing Instrument(string correlationId, [CallerMemberName] string methodName = null)
		{
			var typeName = GetType().Name;
			_logger.Trace(correlationId, "Calling {0} method of {1}", methodName, typeName);
			_counters.IncrementOne(typeName + "." + methodName + ".call_count");
			return _counters.BeginTiming(typeName + "." + methodName + ".call_time");
		}

		/// <summary>
		/// Adds instrumentation to error handling.
		/// </summary>
		/// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
		/// <param name="methodName">a method name.</param>
		/// <param name="ex">Error that occured during the method call</param>
		/// <param name="rethrow">True to throw the exception</param>
		protected void InstrumentError(string correlationId, [CallerMemberName] string methodName = null, Exception ex = null, bool rethrow = false)
		{
			var typeName = GetType().Name;
			_logger.Error(correlationId, ex, "Failed to call {0} method of {1}", methodName, typeName);
			_counters.IncrementOne(typeName + "." + methodName + ".call_errors");

			if (rethrow)
				throw ex;
		}

	}
}
