using Hades.Broker.Abstraction;
using Hades.Broker.Abstraction.Configurations;
using Hades.Broker.Abstraction.Configurators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Broker.Internal.Configurators
{
    internal class ServiceCollectionHadesBrokerConfigurator :
        BrokerServiceCollectionConfigurator,
        IHadesBrokerConfigurator
    {
        public ServiceCollectionHadesBrokerConfigurator(
            IServiceCollection collection, 
            IBrokerConfigurationBuilder builder)
            : base(collection, builder)
        {
            collection.TryAddSingleton<IHadesBroker, HadesBroker>();
        }
    }
}
