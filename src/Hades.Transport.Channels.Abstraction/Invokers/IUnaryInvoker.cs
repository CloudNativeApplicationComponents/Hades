using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Invokers
{
    public interface IUnaryInvoker :
        IHadesInvoker
    {
        Task<Envelope> CallAsync(
            Envelope request, 
            CancellationToken cancellationToken = default);
    }
}