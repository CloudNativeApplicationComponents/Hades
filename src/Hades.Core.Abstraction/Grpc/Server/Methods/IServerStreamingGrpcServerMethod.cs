using Google.Protobuf;
using Grpc.Core;
using System.Collections.Generic;

namespace Hades.Core.Abbstraction.Grpc.Server
{
    public interface IServerStreamingGrpcServerMethod<TRequest, TResponse> : IGrpcServerMethod
        where TRequest : IMessage<TRequest>
        where TResponse : IMessage<TResponse>
    {
        IAsyncEnumerable<TResponse> Execute(TRequest request, ServerCallContext context);
    }
}
