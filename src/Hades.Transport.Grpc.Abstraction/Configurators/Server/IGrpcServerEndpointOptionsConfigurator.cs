using Hades.Transport.Grpc.Abstraction.Options;
using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcServerEndpointOptionsConfigurator
    {
        IGrpcServerEndpointOptionsConfigurator Configure(Action<GrpcServerEndpointOptions> configure);
        IGrpcServerEndpointOptionsConfigurator WithServiceName(string serviceName);
        IGrpcServerEndpointOptionsConfigurator WithMethodName(string methodName);
    }
}
