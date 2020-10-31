using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using PipServices3.Rpc.Clients;
using PipServices3.Rpc.Services;
using Xunit;

namespace PipServices3.Grpc.Services
{
    public class DummyRestServiceTest : IDisposable
    {
        private readonly DummyGrpcClient client;
        private readonly DummyGrpcService service;
        private readonly string correlationId;

        public DummyRestServiceTest()
        {
            correlationId = IdGenerator.NextLong();

            var config = ConfigParams.FromTuples(
                "connection.protocol", "http",
                "connection.host", "localhost",
                "connection.port", 3000
            );

            client = new DummyGrpcClient();
            client.Configure(config);

            service = new DummyGrpcService();
            service.Configure(config);

            var references = References.FromTuples(
                new Descriptor("pip-services3-dummies", "controller", "default", "default", "1.0"), new DummyController()
            );

            service.SetReferences(references);
            service.OpenAsync(null).Wait();

            client.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            client.CloseAsync(null).Wait();
            service.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task CRUD_Operations()
        {
            await It_Should_Be_Opened();
            await It_Should_Create_Dummy();
            await It_Should_Create_Dummy2();
            await It_Should_Update_Dummy2();
            await It_Should_Get_Dummy();
            await It_Should_Get_Dummies();
            await It_Should_Delete_Dummy();
        }

        private async Task It_Should_Delete_Dummy()
        {
            var existingDummy = new Dummy("1", "Key 1", "Content 1");

            var resultDummy = await client.DeleteByIdAsync(correlationId, existingDummy.Id);

            Assert.NotNull(resultDummy);
            Assert.NotNull(resultDummy.Id);
            Assert.Equal(existingDummy.Key, resultDummy.Key);
            Assert.Equal(existingDummy.Content, resultDummy.Content);

            var result = await client.GetOneByIdAsync(correlationId, existingDummy.Id);

            Assert.Null(result);
        }

        private async Task It_Should_Be_Opened()
        {
            Assert.True(service.IsOpen());
            Assert.True(client.IsOpen());
        }

        private async Task It_Should_Create_Dummy()
        {
            var newDummy = new Dummy("1", "Key 1", "Content 1");

            var resultDummy = await client.CreateAsync(correlationId, newDummy);

            Assert.NotNull(resultDummy);
            Assert.NotNull(resultDummy.Id);
            Assert.Equal(newDummy.Key, resultDummy.Key);
            Assert.Equal(newDummy.Content, resultDummy.Content);
        }

        private async Task It_Should_Create_Dummy2()
        {
            var newDummy = new Dummy("2", "Key 2", "Content 2");

            var resultDummy = await client.CreateAsync(correlationId, newDummy);

            Assert.NotNull(resultDummy);
            Assert.NotNull(resultDummy.Id);
            Assert.Equal(newDummy.Key, resultDummy.Key);
            Assert.Equal(newDummy.Content, resultDummy.Content);
        }
        
        private async Task It_Should_Update_Dummy2()
        {
            var dummy = new Dummy("2", "Key 2", "Content 3");

            var resultDummy = await client.UpdateAsync(correlationId, dummy);

            Assert.NotNull(resultDummy);
            Assert.NotNull(resultDummy.Id);
            Assert.Equal(dummy.Key, resultDummy.Key);
            Assert.Equal(dummy.Content, resultDummy.Content);
        }

        private async Task It_Should_Get_Dummy()
        {
            var existingDummy = new Dummy("1", "Key 1", "Content 1");

            var resultDummy = await client.GetOneByIdAsync(correlationId, existingDummy.Id);

            Assert.NotNull(resultDummy);
            Assert.NotNull(resultDummy.Id);
            Assert.Equal(existingDummy.Key, resultDummy.Key);
            Assert.Equal(existingDummy.Content, resultDummy.Content);
        }

        private async Task It_Should_Get_Dummies()
        {
            var existingDummy = new Dummy("1", "Key 1", "Content 1");
            
            var resultDummies = await client.GetPageByFilterAsync(correlationId, FilterParams.FromTuples("key", existingDummy.Key), new PagingParams(0, 100));

            Assert.NotNull(resultDummies);
            Assert.NotNull(resultDummies.Data);
            Assert.Single(resultDummies.Data);
            
            resultDummies = await client.GetPageByFilterAsync(correlationId, new FilterParams(), new PagingParams(0, 100));

            Assert.NotNull(resultDummies);
            Assert.NotNull(resultDummies.Data);
            Assert.Equal(2, resultDummies.Data.Count());
        }
    }
}