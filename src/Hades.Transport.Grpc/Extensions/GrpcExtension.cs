using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Internal.Configurators;
using System;

namespace Hades.Transport.Grpc
{
    public static class GrpcExtension
    {
        public static IHadesTransportConfigurator UseGrpc(
            this IHadesTransportConfigurator transportConfigurator,
            Action<IGrpcTransportEndpointConfigurator>? configure = null)
        {
            _ = transportConfigurator
                ?? throw new ArgumentNullException(nameof(transportConfigurator));

            if (transportConfigurator is ServiceCollectionConfigurator serviceCollectionConfigurator)
            {
                var serviceCollection = serviceCollectionConfigurator.ServiceCollection;
                var configurator = new ServiceCollectionGrpcTransportEndpointConfigurator(serviceCollection);
                configure?.Invoke(configurator);
                return transportConfigurator;
            }
            else
            {
                //TODO raise specifice error
                throw new ArgumentException($"The {nameof(transportConfigurator)} is not an instance of ServiceCollectionConfigurator.", nameof(transportConfigurator));
            }
        }
    }
}
