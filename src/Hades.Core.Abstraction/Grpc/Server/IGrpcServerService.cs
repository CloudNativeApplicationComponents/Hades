using Google.Protobuf;
using Grpc.Core;
using System.Collections.Generic;

namespace Hades.Core.Abbstraction.Grpc.Server
{
    public interface IGrpcServerService
    {
        string Name { get; }
        IEnumerable<IGrpcServerMethod> Methods { get; }
        ServerServiceDefinition Build();

        void Visit<TRequest, TResponse>(IUnaryGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new();


        void Visit<TRequest, TResponse>(IClientStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new();

        void Visit<TRequest, TResponse>(IServerStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new();

        void Visit<TRequest, TResponse>(IDuplexStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new();
    }
}
