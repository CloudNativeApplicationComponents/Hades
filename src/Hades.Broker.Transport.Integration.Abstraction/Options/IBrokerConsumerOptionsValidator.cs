namespace Hades.Broker.Transport.Integration.Abstraction.Options
{
    public interface IBrokerConsumerOptionsValidator
    {
        bool Validate(BrokerConsumerOptions options);
    }
}
