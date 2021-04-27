using System.Threading;
using System.Threading.Tasks;

namespace Hades.Broker.Abstraction
{
    public interface IHadesProducer
    {
        Task<DeliveryResult> ProduceAsync(HadesTopic topic, IHadesMessage message, CancellationToken cancellationToken = default);
    }
}
