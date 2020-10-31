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
    public sealed class DummyCommandableHttpClientTest : IDisposable
    {
        private static readonly ConfigParams RestConfig = ConfigParams.FromTuples(
            "connection.uri", "http://localhost:3000",
            "options.timeout", 15000
            //"connection.protocol", "http",
            //"connection.host", "localhost",
            //"connection.port", 3000
        );

        private readonly DummyController _ctrl;
        private readonly DummyCommandableGrpcClient _client;
        private readonly DummyClientFixture _fixture;
        private readonly CancellationTokenSource _source;

        private readonly DummyCommandableGrpcService _service;

        public DummyCommandableHttpClientTest()
        {
            _ctrl = new DummyController();

            _service = new DummyCommandableGrpcService();

            _client = new DummyCommandableGrpcClient();

            var references = References.FromTuples(
                new Descriptor("pip-services3-dummies", "controller", "default", "default", "1.0"), _ctrl,
                new Descriptor("pip-services3-dummies", "service", "rest", "default", "1.0"), _service,
                new Descriptor("pip-services3-dummies", "client", "rest", "default", "1.0"), _client
            );
            _service.Configure(RestConfig);
            _client.Configure(RestConfig);

            _client.SetReferences(references);
            _service.SetReferences(references);

            _service.OpenAsync(null).Wait();

            _fixture = new DummyClientFixture(_client);

            _source = new CancellationTokenSource();

            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public void TestCrudOperations()
        {
            var task = _fixture.TestCrudOperations();
            task.Wait();
        }

        public void Dispose()
        {
            var task = _client.CloseAsync(null);
            task.Wait();

            task = _service.CloseAsync(null);
            task.Wait();
        }
    }
}
