using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcTransportServerConfigurator
    {
        IGrpcTransportServerConfigurator WithOptions(Action<IGrpcServerOptionsConfigurator> configure);
        IGrpcTransportServerConfigurator AddEndpoint(Action<IGrpcTransportServerEndpointConfigurator> configure);
    }
}
