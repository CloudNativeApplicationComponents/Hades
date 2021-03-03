using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcTransportClientConfigurator
    {
        IGrpcTransportClientConfigurator AddEndpoint(Action<IGrpcTransportClientEndpointConfigurator> configure);
    }
}
