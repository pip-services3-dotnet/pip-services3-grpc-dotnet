using PipServices3.Commons.Refer;
using PipServices3.Grpc.Services;

namespace PipServices3.Rpc.Services
{
    public sealed class DummyCommandableGrpcService : CommandableGrpcService
    {
        public DummyCommandableGrpcService(string name = null) 
            : base(name)
        {
            _dependencyResolver.Put("controller", new Descriptor("pip-services3-dummies", "controller", "default", "*", "1.0"));
        }
    }
}
