using Google.Protobuf;
using Grpc.Core;

namespace Hades.Transport.Grpc
{
    public interface IGrpcMethodFactory
    {
        Method<TRequest, TResponse> Create<TRequest, TResponse>(
            string serviceName,
            string methodName,
            MethodType methodType)
            where TRequest : class, IMessage<TRequest>, new()
            where TResponse : class, IMessage<TResponse>, new();
    }
}
