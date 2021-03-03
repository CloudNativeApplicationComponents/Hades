using Google.Protobuf;
using Grpc.Core;
using Hades.Core.Abbstraction.Grpc.Server;
using System;

namespace Hades.Core.Grpc
{
    public abstract class GrpcServerMethodBase<TRequest, TResponse> : IGrpcServerMethod
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
    {
        public string Name { get; }

        public abstract MethodType MethodType { get; }

        public GrpcServerMethodBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
        public abstract void Bind(IGrpcServerService grpcService, ServerServiceDefinition.Builder builder);
    }
}
