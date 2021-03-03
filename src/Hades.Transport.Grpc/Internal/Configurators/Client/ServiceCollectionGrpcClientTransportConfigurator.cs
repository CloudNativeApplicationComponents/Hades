using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcClientTransportConfigurator :
        ServiceCollectionConfigurator,
        IGrpcTransportClientConfigurator
    {
        public ServiceCollectionGrpcClientTransportConfigurator(
            IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
        }

        public IGrpcTransportClientConfigurator AddEndpoint(Action<IGrpcTransportClientEndpointConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcClientTransportEndpointConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }
    }
}
