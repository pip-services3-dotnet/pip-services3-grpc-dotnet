using Dummies;
using Grpc.Core;
using PipServices3.Commons.Data;
using PipServices3.Grpc;
using PipServices3.Grpc.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;
using IternalDummy = Dummies.Dummy;
using PublicDummy = PipServices3.Grpc.Dummy;

namespace PipServices3.Rpc.Clients
{
	public class DummyGrpcClient : GrpcClient<Dummies.Dummies.DummiesClient>, IDummyClient
	{
		public async Task<PublicDummy> CreateAsync(string correlationId, PublicDummy entity)
		{
			var request = new DummyObjectRequest
			{
				CorrelationId = correlationId,
				Dummy = ConvertFromPublic(entity)
			};

			var item = await _client.create_dummyAsync(request, new CallOptions());
			return ConvertToPublic(item);
		}

		public async Task<PublicDummy> DeleteByIdAsync(string correlationId, string id)
		{
			var request = new DummyIdRequest
			{
				CorrelationId = correlationId,
				DummyId = id
			};

			var item = await _client.delete_dummy_by_idAsync(request, new CallOptions());

			return ConvertToPublic(item);
		}

		public async Task<PublicDummy> GetOneByIdAsync(string correlationId, string id)
		{
			var request = new DummyIdRequest
			{
				CorrelationId = correlationId,
				DummyId = id
			};

			var item = await _client.get_dummy_by_idAsync(request, new CallOptions());

			return ConvertToPublic(item);
		}

		public async Task<DataPage<PublicDummy>> GetPageByFilterAsync(string correlationId, FilterParams filter, Commons.Data.PagingParams paging)
		{
			var request = new DummiesPageRequest
			{
				CorrelationId = correlationId,
				Paging = new Dummies.PagingParams()
			};
     		request.Filter.Add(filter);
			if (paging.Skip.HasValue) request.Paging.Skip = paging.Skip.Value;
			if (paging.Take.HasValue) request.Paging.Take = Convert.ToInt32(paging.Take.Value);

			var page = await _client.get_dummiesAsync(request, new CallOptions());

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

			var item = await _client.update_dummyAsync(request, new CallOptions());
			return ConvertToPublic(item);
		}

		private static PublicDummy ConvertToPublic(IternalDummy dummy)
		{
			if (dummy == null || dummy.Id == "") return null;
			return new PublicDummy
			{
				Id = dummy.Id,
				Key = dummy.Key,
				Content = dummy.Content
			};
		}

		private static IternalDummy ConvertFromPublic(PublicDummy dummy)
		{
			if (dummy == null) return null;
			return new IternalDummy
			{
				Id = dummy.Id,
				Key = dummy.Key,
				Content = dummy.Content
			};
		}
	}
}
