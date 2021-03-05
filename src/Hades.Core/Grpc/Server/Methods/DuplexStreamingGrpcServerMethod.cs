using Google.Protobuf;
using Grpc.Core;
using Hades.Core.Abbstraction.Grpc.Server;
using System;
using System.Collections.Generic;

namespace Hades.Core.Grpc.Server
{
    public class DuplexStreamingGrpcServerMethod<TRequest, TResponse> :
        GrpcServerMethodBase<TRequest, TResponse>, IDuplexStreamingGrpcServerMethod<TRequest, TResponse>
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
    {
        public override MethodType MethodType => MethodType.DuplexStreaming;
        private readonly Func<IAsyncEnumerable<TRequest>, IAsyncEnumerable<TResponse>> _func;
    
        public DuplexStreamingGrpcServerMethod(string name, Func<IAsyncEnumerable<TRequest>, IAsyncEnumerable<TResponse>> func) : base(name)
        {
            _func = func
              ?? throw new ArgumentNullException(nameof(func));
        }

        public virtual IAsyncEnumerable<TResponse> Execute(IAsyncEnumerable<TRequest> request, ServerCallContext context)
        {
            return _func(request);
        }

        public override void Bind(IGrpcServerService grpcService, ServerServiceDefinition.Builder builder)
        {
            grpcService.Visit(this, builder);
        }
    }
}
