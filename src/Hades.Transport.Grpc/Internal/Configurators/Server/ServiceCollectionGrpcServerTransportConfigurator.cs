using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Internal.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcServerTransportConfigurator :
        ServiceCollectionConfigurator,
        IGrpcTransportServerConfigurator
    {
        public ServiceCollectionGrpcServerTransportConfigurator(
            IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
            ServiceCollection
                .TryAddSingleton<GrpcServerDispatcherAgent>();
        }

        public IGrpcTransportServerConfigurator AddEndpoint(Action<IGrpcTransportServerEndpointConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcServerTransportEndpointConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }

        public IGrpcTransportServerConfigurator WithOptions(Action<IGrpcServerOptionsConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcServerOptionsConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }
    }
}
