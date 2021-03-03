using Hades.Broker.Transport.Integration.Abstraction.Options;

namespace Hades.Broker.Transport.Integration.Internal.Options
{
    internal class BrokerProducerOptionsValidator : IBrokerProducerOptionsValidator
    {
        public bool Validate(BrokerProducerOptions options)
        {
            return options.Topic != null;
        }
    }
}
