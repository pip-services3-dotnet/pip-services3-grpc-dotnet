﻿using System;
using System.Threading.Tasks;

using PipServices3.Commons.Commands;
using PipServices3.Commons.Run;
using PipServices3.Commons.Validate;
using PipServices3.Commons.Data;

namespace PipServices3.Grpc
{
    public class DummyCommandSet : CommandSet
    {
        private IDummyController _controller;

        public DummyCommandSet(IDummyController controller)
        {
            _controller = controller;

            AddCommand(MakeGetPageByFilterCommand());
            AddCommand(MakeGetOneByIdCommand());
            AddCommand(MakeCreateCommand());
            AddCommand(MakeUpdateCommand());
            AddCommand(MakeDeleteByIdCommand());
        }

        private ICommand MakeGetPageByFilterCommand()
        {
            return new Command(
                "get_dummies",
                new ObjectSchema()
                    .WithOptionalProperty("correlation_id", typeof(string))
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, args) => 
                {
                    var filter = FilterParams.FromValue(args.Get("filter"));
                    var paging = PagingParams.FromValue(args.Get("paging"));

                    return await _controller.GetPageByFilterAsync(correlationId, filter, paging);    
                });
        }

        private ICommand MakeGetOneByIdCommand()
        {
            return new Command(
                "get_dummy_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("dummy_id", Commons.Convert.TypeCode.String),
                async (correlationId, args) => 
                {
                    var dummyId = args.GetAsString("dummy_id");
                    return await _controller.GetOneByIdAsync(correlationId, dummyId);                    
                });
        }

        private ICommand MakeCreateCommand()
        {
            return new Command(
                "create_dummy",
                new ObjectSchema()
                    .WithRequiredProperty("dummy", new DummySchema()),
                async (correlationId, args) => 
                {
                    var dummy = ExtractDummy(args);
                    return await _controller.CreateAsync(correlationId, dummy);
                });
        }

        private ICommand MakeUpdateCommand()
        {
            return new Command(
                "update_dummy",
                new ObjectSchema()
                    .WithRequiredProperty("dummy", new DummySchema()),
                async (correlationId, args) =>
                {
                    var dummy = ExtractDummy(args);
                    return await _controller.UpdateAsync(correlationId, dummy);
                });
        }

        private ICommand MakeDeleteByIdCommand()
        {
            return new Command(
                "delete_dummy",
                new ObjectSchema()
                    .WithRequiredProperty("dummy_id", Commons.Convert.TypeCode.String),
                async (correlationId, args) => 
                {
                    var dummyId = args.GetAsString("dummy_id");

                    return await _controller.DeleteByIdAsync(correlationId, dummyId);
                });
        }

        private static Dummy ExtractDummy(Parameters args)
        {
            var map = args.GetAsMap("dummy");

            var id = map.GetAsStringWithDefault("id", string.Empty);
            var key = map.GetAsStringWithDefault("key", string.Empty);
            var content = map.GetAsStringWithDefault("content", string.Empty);
            var flag = map.GetAsBooleanWithDefault("flag", false);

            var dummy = new Dummy(id, key, content);
            return dummy;
        }

    }
}