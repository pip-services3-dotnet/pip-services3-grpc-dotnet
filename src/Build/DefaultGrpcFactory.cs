using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using PipServices3.Grpc.Services;

namespace PipServices3.Grpc.Build
{
    /// <summary>
    /// Creates GRPC components by their descriptors.
    /// </summary>
    /// See <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/class_pip_services_1_1_components_1_1_build_1_1_factory.html">Factory</a>, 
    /// <a href="https://pip-services3-dotnet.github.io/pip-services3-rpc-dotnet/class_pip_services_1_1_rpc_1_1_services_1_1_http_endpoint.html">HttpEndpoint</a>, 
    /// <a href="https://pip-services3-dotnet.github.io/pip-services3-rpc-dotnet/class_pip_services_1_1_rpc_1_1_services_1_1_status_rest_service.html">StatusRestService</a>, 
    /// <a href="https://pip-services3-dotnet.github.io/pip-services3-rpc-dotnet/class_pip_services_1_1_rpc_1_1_services_1_1_heartbeat_rest_service.html">HeartbeatRestService</a>
    public class DefaultGrpcFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("pip-services", "factory", "grpc", "default", "1.0");
        public static Descriptor Descriptor3 = new Descriptor("pip-services3", "factory", "grpc", "default", "1.0");
        public static Descriptor GrpcEndpointDescriptor = new Descriptor("pip-services", "endpoint", "grpc", "*", "1.0");
        public static Descriptor GrpcEndpoint3Descriptor = new Descriptor("pip-services3", "endpoint", "grpc", "*", "1.0");

        /// <summary>
        /// Create a new instance of the factory.
        /// </summary>
        public DefaultGrpcFactory()
        {
            RegisterAsType(GrpcEndpointDescriptor, typeof(GrpcEndpoint));
            RegisterAsType(GrpcEndpoint3Descriptor, typeof(GrpcEndpoint));
        }
    }
}
