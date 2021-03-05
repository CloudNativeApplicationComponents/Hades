using Google.Protobuf;
using Grpc.Core;
using Hades.Core.Abbstraction.Grpc.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hades.Core.Grpc.Server
{
    public class ClientStreamingGrpcServerMethod<TRequest, TResponse> :
        GrpcServerMethodBase<TRequest, TResponse>, IClientStreamingGrpcServerMethod<TRequest, TResponse>
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
    {
        public override MethodType MethodType => MethodType.ClientStreaming;
        private readonly Func<IAsyncEnumerable<TRequest>, Task<TResponse>> _func;

        public ClientStreamingGrpcServerMethod(string name, Func<IAsyncEnumerable<TRequest>, Task<TResponse>> func) : base(name)
        {
            _func = func
              ?? throw new ArgumentNullException(nameof(func));
        }
        public async virtual Task<TResponse> Execute(IAsyncEnumerable<TRequest> request, ServerCallContext context)
        {
            return await _func(request);
        }

        public override void Bind(IGrpcServerService grpcService, ServerServiceDefinition.Builder builder)
        {
            grpcService.Visit(this, builder);
        }
    }
}
