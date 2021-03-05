using Grpc.Core;

namespace Hades.Core.Abbstraction.Grpc.Server
{

    public interface IGrpcServerMethod
    {
        string Name { get; }
        MethodType MethodType { get; }
        void Bind(IGrpcServerService grpcService, ServerServiceDefinition.Builder builder);
    }
}
