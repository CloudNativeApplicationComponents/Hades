using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Broker.Abstraction
{
    public interface IHadesConsumer
    {
        Task ConsumeAsync(HadesTopic hadesTopic, IHadesMessage message, CancellationToken cancellationToken = default);
    }
}
