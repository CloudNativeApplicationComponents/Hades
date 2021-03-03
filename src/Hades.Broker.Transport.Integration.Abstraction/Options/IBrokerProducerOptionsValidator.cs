namespace Hades.Broker.Transport.Integration.Abstraction.Options
{
    public interface IBrokerProducerOptionsValidator
    {
        bool Validate(BrokerProducerOptions options);
    }
}
