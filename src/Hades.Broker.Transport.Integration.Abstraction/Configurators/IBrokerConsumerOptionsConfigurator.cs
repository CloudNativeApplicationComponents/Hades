using Hades.Broker.Abstraction;
using Hades.Broker.Transport.Integration.Abstraction.Options;
using System;

namespace Hades.Broker.Transport.Integration.Abstraction.Configurators
{
    public interface IBrokerConsumerOptionsConfigurator
    {
        IBrokerConsumerOptionsConfigurator Configure(Action<BrokerConsumerOptions> configure);
        IBrokerConsumerOptionsConfigurator WithTopic(HadesTopic topic);
        IBrokerConsumerOptionsConfigurator WithGroup(string group);
    }
}
