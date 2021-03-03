using Google.Protobuf;
using Grpc.Core;
using Hades.Core.Abbstraction.Grpc.Server;
using System;
using System.Collections.Generic;

namespace Hades.Core.Grpc.Server
{
    public class ServerStreamingGrpcServerMethod<TRequest, TResponse> :
        GrpcServerMethodBase<TRequest, TResponse>, IServerStreamingGrpcServerMethod<TRequest, TResponse>
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
    {
        public override MethodType MethodType => MethodType.ServerStreaming;
        private readonly Func<TRequest, IAsyncEnumerable<TResponse>> _func;

        public ServerStreamingGrpcServerMethod(string name, Func<TRequest, IAsyncEnumerable<TResponse>> func)
            : base(name)
        {
            _func = func
                ?? throw new ArgumentNullException(nameof(func));
        }

        public virtual IAsyncEnumerable<TResponse> Execute(TRequest request, ServerCallContext context)
        {
            return _func(request);
        }

        public override void Bind(IGrpcServerService grpcService, ServerServiceDefinition.Builder builder)
        {
            grpcService.Visit(this, builder);
        }
    }
}
