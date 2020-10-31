using Commandable;
using Grpc.Core;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data.Mapper;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Run;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices3.Grpc.Services
{
	internal class CommandableGrpcServerService : Commandable.Commandable.CommandableBase
    {
		readonly Dictionary<string, Func<string, Parameters, Task<object>>> _commandableMethods;

        internal CommandableGrpcServerService(Dictionary<string, Func<string, Parameters, Task<object>>> commandableMethods)
        {
            _commandableMethods = commandableMethods;
        }

        public override async Task<InvokeReply> invoke(InvokeRequest request, ServerCallContext context)
        {
            var method = request.Method;
            var correlationId = request.CorrelationId;
            var action = _commandableMethods?[method];

            // Handle method not found
            if (action == null)
            {
                var err = new InvocationException(correlationId, "METHOD_NOT_FOUND", "Method " + method + " was not found")
                    .WithDetails("method", method);

                return CreateErrorResponse(err);
            }

            try
            {
                // Convert arguments
                var argsEmpty = request.ArgsEmpty;
                var argsJson = request.ArgsJson;
                var args = !argsEmpty && !string.IsNullOrWhiteSpace(argsJson)
                    ? Parameters.FromJson(argsJson)
                    : new Parameters();

                // Todo: Validate schema
                //var schema = this._commandableSchemas[method];
                //if (schema)
                //{
                //    //...
                //}

                // Call command action
                var result = await action(correlationId, args);

				// Process result and generate response
				var response = new InvokeReply
				{
					Error = null,
					ResultEmpty = result == null
				};

				if (result != null)
                {
                    response.ResultJson = JsonConverter.ToJson(result);
                }

                return response;
            }
            catch (Exception ex)
            {
                // Handle unexpected exception
                var err = new InvocationException(correlationId, "METHOD_FAILED", "Method " + method + " failed")
                    .Wrap(ex).WithDetails("method", method);

                return CreateErrorResponse(err);
            }
        }

        private InvokeReply CreateErrorResponse(Exception ex)
        {
            return new InvokeReply
            {
                Error = ObjectMapper.MapTo<Commandable.ErrorDescription>(ErrorDescriptionFactory.Create(ex)),
                ResultEmpty = true,
                ResultJson = null
            };
        }
    }
}
