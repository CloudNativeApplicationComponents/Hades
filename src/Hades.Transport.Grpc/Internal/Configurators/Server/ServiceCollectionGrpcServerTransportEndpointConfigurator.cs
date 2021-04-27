using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcServerTransportEndpointConfigurator :
        ServiceCollectionConfigurator,
        IGrpcTransportServerEndpointConfigurator
    {
        public ServiceCollectionGrpcServerTransportEndpointConfigurator(
            IServiceCollection serviceCollection)
            : base(serviceCollection)
        {

        }

        public IGrpcTransportServerEndpointConfigurator WithOptions(Action<IGrpcServerEndpointOptionsConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcServerEndpointOptionsConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }
    }
}
