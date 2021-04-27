using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Broker.Abstraction.Configurators;
using Hades.Broker.Transport.Integration.Abstraction.Options;
using Hades.Broker.Transport.Integration.Internal.Options;
using Hades.Transport;
using Hades.Transport.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Hades.Broker.Transport.Integration
{
    public static class BrokerExtensions
    {
        public static IHadesBrokerConfigurator UseTransport(
            this IHadesBrokerConfigurator brokerConfigurator,
            Action<IHadesTransportConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            if (brokerConfigurator is ServiceCollectionConfigurator serviceCollectionConfigurator)
            {
                var collection = serviceCollectionConfigurator.ServiceCollection;

                collection.TryAddSingleton<IBrokerConsumerOptionsValidator, BrokerConsumerOptionsValidator>();
                collection.TryAddSingleton<IBrokerProducerOptionsValidator, BrokerProducerOptionsValidator>();

                collection.UseHadesTransport(cg =>
                {
                    configure(cg);
                });
            }
            else
            {
                //TODO raise specifice error
                throw new ArgumentException($"The {nameof(brokerConfigurator)} is not an instance of ServiceCollectionConfigurator.", nameof(brokerConfigurator));
            }

            return brokerConfigurator;
        }
    }
}
