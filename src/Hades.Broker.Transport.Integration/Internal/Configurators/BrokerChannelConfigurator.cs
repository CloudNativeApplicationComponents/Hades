using Hades.Broker.Abstraction.Configurations;
using System;

namespace Hades.Broker.Transport.Integration.Internal.Configurators
{
    internal abstract class BrokerChannelConfigurator
    {
        protected IBrokerConfigurationBuilder Builder { get; }

        protected BrokerChannelConfigurator(IBrokerConfigurationBuilder builder)
        {
            Builder = builder
                ?? throw new ArgumentNullException(nameof(builder));
        }
    }
}
