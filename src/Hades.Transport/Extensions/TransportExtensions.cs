using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Internal.Configurators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hades.Transport
{
    public static class TransportExtensions
    {
        public static IHostBuilder UseHadesTransport(
            this IHostBuilder builder,
            Action<IHadesTransportConfigurator>? configure = null)
        {
            builder.ConfigureServices((context, collection) =>
            {
                UseHadesTransport(collection, configure);
            });

            return builder;
        }

        public static IServiceCollection UseHadesTransport(
            this IServiceCollection collection,
            Action<IHadesTransportConfigurator>? configure = null)
        {
            var configurator = new ServiceCollectionHadesTransportConfigurator(collection);

            configure?.Invoke(configurator);

            return collection;
        }
    }
}
