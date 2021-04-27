using Grpc.Core;
using Hades.Transport.Grpc.Abstraction.Options;
using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcClientEndpointOptionsConfigurator
    {
        IGrpcClientEndpointOptionsConfigurator Configure(Action<GrpcClientEndpointOptions> configure);
        IGrpcClientEndpointOptionsConfigurator WithServiceName(string serviceName);
        IGrpcClientEndpointOptionsConfigurator WithMethodName(string methodName);
        IGrpcClientEndpointOptionsConfigurator WithBaseUri(Uri baseUri);
        IGrpcClientEndpointOptionsConfigurator WithCredentials(ChannelCredentials credentials);
    }
}
