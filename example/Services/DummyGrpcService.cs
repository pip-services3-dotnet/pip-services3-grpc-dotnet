using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using PipServices3.Commons.Refer;
using PipServices3.Grpc.Protos;
using ProtoDummy = PipServices3.Grpc.Protos.Dummy;

namespace PipServices3.Grpc.Services
{
	public class DummyGrpcService : GrpcService
    {
        private IDummyController _controller;

        public DummyGrpcService()
            : base("dummies")
        {
            _dependencyResolver.Put("controller", new Descriptor("pip-services3-dummies", "controller", "default", "*", "*"));
        }

        public override void SetReferences(IReferences references)
        {
            base.SetReferences(references);

            _controller = _dependencyResolver.GetOneRequired<IDummyController>("controller");
        }

        public async Task<DummiesPage> GetDummiesAsync(DummiesPageRequest request, ServerCallContext context)
        {
            var correlationId = request.CorrelationId;
            var filter = new Commons.Data.FilterParams(request.Filter);
            var paging = new Commons.Data.PagingParams(request.Paging.Skip, request.Paging.Take, request.Paging.Total);

            var page = await _controller.GetPageByFilterAsync(correlationId, filter, paging);

            var data = new Google.Protobuf.Collections.RepeatedField<ProtoDummy>();

            var response = new DummiesPage { Total = page.Total ?? 0 };
            response.Data.AddRange(page.Data.Select(x => ConvertToPublic(x)));

            return response;
        }

        public async Task<ProtoDummy> GetDummyByIdAsync(DummyIdRequest request, ServerCallContext context)
        {
            var item = await _controller.GetOneByIdAsync(request.CorrelationId, request.DummyId);
            return ConvertToPublic(item);
        }

        public async Task<ProtoDummy> CreateDummyAsync(DummyObjectRequest request, ServerCallContext context)
        {
            var item = await _controller.CreateAsync(request.CorrelationId, ConvertFromPublic(request.Dummy));
            return ConvertToPublic(item);
        }

        public async Task<ProtoDummy> UpdateDummyAsync(DummyObjectRequest request, ServerCallContext context)
        {
            var item = await _controller.UpdateAsync(request.CorrelationId, ConvertFromPublic(request.Dummy));
            return ConvertToPublic(item);
        }

        public async Task<ProtoDummy> DeleteDummyByIdAsync(DummyIdRequest request, ServerCallContext context)
        {
            var item = await _controller.DeleteByIdAsync(request.CorrelationId, request.DummyId);
            return ConvertToPublic(item);
        }

        private static ProtoDummy ConvertToPublic(Dummy dummy)
        {
            if (dummy == null) return null;
            return new ProtoDummy
            {
                Id = dummy.Id,
                Key = dummy.Key,
                Content = dummy.Content
            };
        }

        private static Dummy ConvertFromPublic(ProtoDummy dummy)
        {
            if (dummy == null) return null;
            return new Dummy
            {
                Id = dummy.Id,
                Key = dummy.Key,
                Content = dummy.Content
            };
        }

        protected override void OnRegister()
        {
            RegisterMethod<DummiesPageRequest, DummiesPage>("get_dummies", GetDummiesAsync);
            RegisterMethod<DummyIdRequest, ProtoDummy>("get_dummy_by_id", GetDummyByIdAsync);
            RegisterMethod<DummyObjectRequest, ProtoDummy>("create_dummy", CreateDummyAsync);
            RegisterMethod<DummyObjectRequest, ProtoDummy>("update_dummy", UpdateDummyAsync);
            RegisterMethod<DummyIdRequest, ProtoDummy>("delete_dummy_by_id", DeleteDummyByIdAsync);
        }
    }
}