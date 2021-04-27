using Grpc.Core;
using Hades.Transport.Grpc.Abstraction.Options;
using System;

namespace Hades.Transport.Grpc.Abstraction.Configurators
{
    public interface IGrpcServerOptionsConfigurator
    {
        IGrpcServerOptionsConfigurator Configure(Action<GrpcServerOptions> configure);
        IGrpcServerOptionsConfigurator Host(string host);
        IGrpcServerOptionsConfigurator Port(int port);
        IGrpcServerOptionsConfigurator Credentials(ServerCredentials serverCredentials);
    }
}
