using Hades.Broker.Transport.Integration.Abstraction.Options;

namespace Hades.Broker.Transport.Integration.Internal.Options
{
    internal class BrokerConsumerOptionsValidator : IBrokerConsumerOptionsValidator
    {
        public bool Validate(BrokerConsumerOptions options)
        {
            return !string.IsNullOrWhiteSpace(options.GroupName) 
                && options.Topic != null;
        }
    }
}
