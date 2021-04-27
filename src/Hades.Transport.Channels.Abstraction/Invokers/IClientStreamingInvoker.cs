using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Invokers
{
    public interface IClientStreamingInvoker :
        IHadesInvoker
    {
        Task<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> stream, 
            CancellationToken cancellationToken = default);
    }
}