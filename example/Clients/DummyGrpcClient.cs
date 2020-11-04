using Grpc.Core;
using PipServices3.Commons.Data;
using PipServices3.Grpc;
using PipServices3.Grpc.Clients;
using PipServices3.Grpc.Protos;
using System;
using System.Linq;
using System.Threading.Tasks;
using static PipServices3.Grpc.Protos.Dummies;
using ProtoDummy = PipServices3.Grpc.Protos.Dummy;
using PublicDummy = PipServices3.Grpc.Dummy;

namespace PipServices3.Rpc.Clients
{
	public class DummyGrpcClient : GrpcClient, IDummyClient
	{
		public DummyGrpcClient()
			: base("dummies")
		{ 
		}

		public async Task<PublicDummy> CreateAsync(string correlationId, PublicDummy entity)
		{
			var request = new DummyObjectRequest
			{
				CorrelationId = correlationId,
				Dummy = ConvertFromPublic(entity)
			};

			var item = await CallAsync<DummyObjectRequest, ProtoDummy>("create_dummy", request);

			return ConvertToPublic(item);
		}

		public async Task<PublicDummy> DeleteByIdAsync(string correlationId, string id)
		{
			var request = new DummyIdRequest
			{
				CorrelationId = correlationId,
				DummyId = id
			};

			var item = await CallAsync<DummyIdRequest, ProtoDummy>("delete_dummy_by_id", request);

			return ConvertToPublic(item);
		}

		public async Task<PublicDummy> GetOneByIdAsync(string correlationId, string id)
		{
			var request = new DummyIdRequest
			{
				CorrelationId = correlationId,
				DummyId = id
			};

			var item = await CallAsync<DummyIdRequest, ProtoDummy>("get_dummy_by_id", request);

			return ConvertToPublic(item);
		}

		public async Task<DataPage<PublicDummy>> GetPageByFilterAsync(string correlationId, FilterParams filter, Commons.Data.PagingParams paging)
		{
			var request = new DummiesPageRequest
			{
				CorrelationId = correlationId,
				Paging = new Grpc.Protos.PagingParams()
			};
     		request.Filter.Add(filter);
			if (paging.Skip.HasValue) request.Paging.Skip = paging.Skip.Value;
			if (paging.Take.HasValue) request.Paging.Take = Convert.ToInt32(paging.Take.Value);

			var page = await CallAsync<DummiesPageRequest, DummiesPage>("get_dummies", request);

			var result = new DataPage<PublicDummy>
			{
				Data = page.Data.Select(x => ConvertToPublic(x)).ToList(),
				Total = page.Total
			};

			return result;
		}

		public async Task<PublicDummy> UpdateAsync(string correlationId, PublicDummy entity)
		{
			var request = new DummyObjectRequest
			{
				CorrelationId = correlationId,
				Dummy = ConvertFromPublic(entity)
			};

			var item = await CallAsync<DummyObjectRequest, ProtoDummy>("update_dummy", request);
			return ConvertToPublic(item);
		}

		private static PublicDummy ConvertToPublic(ProtoDummy dummy)
		{
			if (dummy == null || dummy.Id == "") return null;
			return new PublicDummy
			{
				Id = dummy.Id,
				Key = dummy.Key,
				Content = dummy.Content
			};
		}

		private static ProtoDummy ConvertFromPublic(PublicDummy dummy)
		{
			if (dummy == null) return null;
			return new ProtoDummy
			{
				Id = dummy.Id,
				Key = dummy.Key,
				Content = dummy.Content
			};
		}
	}
}
