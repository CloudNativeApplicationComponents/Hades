using Hades.Broker.Abstraction.Configurations;
using Hades.Broker.Abstraction.Configurators;
using Hades.Broker.Internal.Configurations;
using Hades.Broker.Internal.Configurators;
using Microsoft.Extensions.Hosting;
using System;

namespace Hades.Broker
{
    public static class BrokerExtension
    {
        public static IHostBuilder UseHadesBroker(
            this IHostBuilder builder,
            Action<IHadesBrokerConfigurator>? configure = null)
        {
            builder.ConfigureServices((context, collection) =>
            {
                var configurator = new ServiceCollectionHadesBrokerConfigurator(collection, new BrokerConfigurationBuilder());

                configure?.Invoke(configurator);
            });

            return builder;
        }
    }
}
