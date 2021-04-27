using Hades.Transport.Grpc.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;
using CloudNativeApplicationComponents.Utils.Configurators;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcClientTransportEndpointConfigurator :
        ServiceCollectionConfigurator,
        IGrpcTransportClientEndpointConfigurator
    {
        public ServiceCollectionGrpcClientTransportEndpointConfigurator(
            IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
        }

        public IGrpcTransportClientEndpointConfigurator WithOptions(Action<IGrpcClientEndpointOptionsConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcClientEndpointOptionsConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }
    }
}
