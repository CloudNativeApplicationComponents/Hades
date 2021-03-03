using Hades.Transport.Grpc.Abstraction.Client;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Abstraction.Server;

namespace Hades.Transport.Grpc.Abstraction
{
    public interface IGrpcTransportFactory
    {
        IGrpcTransportClientEndpoint CreateClient(GrpcClientEndpointOptions options);
        IGrpcTransportServerEndpoint CreateServer(GrpcServerEndpointOptions options);
    }
}
