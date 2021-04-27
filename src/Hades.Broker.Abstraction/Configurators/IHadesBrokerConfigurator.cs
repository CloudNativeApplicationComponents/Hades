using Hades.Broker.Abstraction.Configurations;

namespace Hades.Broker.Abstraction.Configurators
{
    public interface IHadesBrokerConfigurator
    {
        IBrokerConfigurationBuilder Builder { get; }
    }
}
