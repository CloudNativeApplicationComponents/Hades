using System.Threading;
using System.Threading.Tasks;

namespace Hades.Broker.Abstraction
{
    public interface IHadesBroker
    {
        void Scheduler(HadesTopic topic, 
            IBrokerScheduler? scheduler);

        IHadesSubscription Subscribe(HadesTopic topic,
            string groupName,
            IHadesConsumer consumer);
        
        Task<DeliveryResult> PublishAsync(HadesTopic topic,
            IHadesMessage message, 
            CancellationToken cancellationToken = default);
        
        Task<DeliveryResult> Send(IHadesSubscription subscription,
            IHadesMessage message,
            CancellationToken cancellationToken = default);
    }
}
