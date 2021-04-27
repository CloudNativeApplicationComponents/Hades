using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Broker.Abstraction.Configurations;
using Hades.Broker.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hades.Broker.Internal.Configurators
{
    internal class BrokerServiceCollectionConfigurator : ServiceCollectionConfigurator
    {
        public IBrokerConfigurationBuilder Builder { get; }

        public BrokerServiceCollectionConfigurator(IServiceCollection collection,
            IBrokerConfigurationBuilder builder)
            :base(collection)
        {
            Builder = builder
               ?? throw new ArgumentNullException(nameof(builder));
        }
    }
}
