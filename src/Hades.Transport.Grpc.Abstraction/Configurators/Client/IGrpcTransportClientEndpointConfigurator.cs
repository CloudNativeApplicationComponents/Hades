using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Client;
using Hades.Transport.Grpc.Abstraction.Options;
using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcTransportClientEndpointConfigurator :
        ITransportClientEndpointConfigurator<IGrpcTransportClientEndpoint, GrpcClientEndpointOptions>
    {
        IGrpcTransportClientEndpointConfigurator WithOptions(Action<IGrpcClientEndpointOptionsConfigurator> configure);
    }
}
