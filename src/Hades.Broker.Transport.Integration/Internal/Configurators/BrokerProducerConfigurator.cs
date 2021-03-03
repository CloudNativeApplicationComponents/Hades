using Hades.Broker.Abstraction;
using Hades.Broker.Transport.Integration.Abstraction.Configurators;
using Hades.Broker.Transport.Integration.Abstraction.Options;
using System;

namespace Hades.Broker.Transport.Integration.Internal.Configurators
{
    internal class BrokerProducerConfigurator : IBrokerProducerOptionsConfigurator
    {
        private Action<BrokerProducerOptions> _configure = _ => { };

        public IBrokerProducerOptionsConfigurator Configure(Action<BrokerProducerOptions> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            _configure += configure;
            return this;
        }


        public IBrokerProducerOptionsConfigurator WithTopic(HadesTopic topic)
        {
            _configure += op => { op.Topic = topic; };
            return this;
        }
    }
}
