using Hades.Broker.Abstraction;
using Hades.Broker.Transport.Integration.Abstraction.Configurators;
using Hades.Broker.Transport.Integration.Abstraction.Options;
using System;

namespace Hades.Broker.Transport.Integration.Internal.Configurators
{
    internal class BrokerConsumerConfigurator : IBrokerConsumerOptionsConfigurator
    {
        private Action<BrokerConsumerOptions> _configure = _ => { };

        public IBrokerConsumerOptionsConfigurator Configure(Action<BrokerConsumerOptions> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            _configure += configure;
            return this;
        }

        public IBrokerConsumerOptionsConfigurator WithGroup(string group)
        {
            _configure += op => { op.GroupName = group; };
            return this;
        }

        public IBrokerConsumerOptionsConfigurator WithTopic(HadesTopic topic)
        {
            _configure += op => { op.Topic = topic; };
            return this;
        }
    }
}
