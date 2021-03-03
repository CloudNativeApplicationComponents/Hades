using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcTransportEndpointConfigurator
    {
        IGrpcTransportEndpointConfigurator UseClient(Action<IGrpcTransportClientConfigurator> configure);
        IGrpcTransportEndpointConfigurator UseServer(Action<IGrpcTransportServerConfigurator> configure);
    }
}
