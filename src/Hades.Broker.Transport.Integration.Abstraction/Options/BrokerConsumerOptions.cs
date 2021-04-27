using Hades.Broker.Abstraction;

namespace Hades.Broker.Transport.Integration.Abstraction.Options
{
    public class BrokerConsumerOptions
    {
        public string? GroupName { get; set; }
        public HadesTopic? Topic { get; set; }
    }
}
