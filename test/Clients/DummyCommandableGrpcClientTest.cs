using System;
using System.Threading;
using System.Threading.Tasks;

using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using PipServices3.Grpc.Services;
using PipServices3.Rpc.Clients;
using PipServices3.Rpc.Services;
using Xunit;

namespace PipServices3.Grpc.Clients
{
    public sealed class DummyCommandableGrpcClientTest : IDisposable
    {
        private static readonly ConfigParams GrpcConfig = ConfigParams.FromTuples(
            "connection.uri", "http://localhost:3000",
            "options.timeout", 15000
            //"connection.protocol", "http",
            //"connection.host", "localhost",
            //"connection.port", 3000
        );

        private readonly DummyController _ctrl;
        private readonly DummyCommandableGrpcClient _client;
        private readonly DummyClientFixture _fixture;

        private readonly DummyCommandableGrpcService _service;

        public DummyCommandableGrpcClientTest()
        {
            _ctrl = new DummyController();

            _service = new DummyCommandableGrpcService();

            _client = new DummyCommandableGrpcClient();

            var references = References.FromTuples(
                new Descriptor("pip-services3-dummies", "controller", "default", "default", "1.0"), _ctrl,
                new Descriptor("pip-services3-dummies", "service", "grpc", "default", "1.0"), _service,
                new Descriptor("pip-services3-dummies", "client", "grpc", "default", "1.0"), _client
            );
            _service.Configure(GrpcConfig);
            _client.Configure(GrpcConfig);

            _client.SetReferences(references);
            _service.SetReferences(references);

            _service.OpenAsync(null).Wait();

            _fixture = new DummyClientFixture(_client);

            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        public void Dispose()
        {
            _client.CloseAsync(null).Wait();
            _service.CloseAsync(null).Wait();
        }
    }
}
