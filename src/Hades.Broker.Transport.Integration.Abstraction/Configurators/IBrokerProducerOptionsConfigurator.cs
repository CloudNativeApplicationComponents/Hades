using Hades.Broker.Abstraction;
using Hades.Broker.Transport.Integration.Abstraction.Options;
using System;

namespace Hades.Broker.Transport.Integration.Abstraction.Configurators
{
    public interface IBrokerProducerOptionsConfigurator
    {
        IBrokerProducerOptionsConfigurator Configure(Action<BrokerProducerOptions> configure);
        IBrokerProducerOptionsConfigurator WithTopic(HadesTopic topic);
    }
}
