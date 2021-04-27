using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Client;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Abstraction.Server;
using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcTransportServerEndpointConfigurator :
        ITransportServerEndpointConfigurator<IGrpcTransportServerEndpoint, GrpcServerEndpointOptions>
    {
        IGrpcTransportServerEndpointConfigurator WithOptions(Action<IGrpcServerEndpointOptionsConfigurator> configure);
    }
}
