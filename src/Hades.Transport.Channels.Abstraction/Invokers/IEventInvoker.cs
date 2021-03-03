using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Invokers
{
    public interface IEventInvoker :
        IHadesInvoker
    {
        Task PublishAsync(Envelope envelope, CancellationToken cancellationToken = default);
    }
}