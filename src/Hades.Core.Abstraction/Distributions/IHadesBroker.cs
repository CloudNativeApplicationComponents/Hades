using System.Threading.Tasks;

namespace Hades.Core.Abstraction.Distributions
{
    public interface IHadesBroker
    {
        IHadesSubscription Subscribe(IHadesConsumer consumer, HadesTopic topic, string group);
        Task Publish(Envelope envelope);
    }
}
