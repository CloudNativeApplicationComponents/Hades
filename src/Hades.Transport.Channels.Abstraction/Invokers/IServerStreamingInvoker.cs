using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Channels.Abstraction.Invokers
{
    public interface IServerStreamingInvoker :
        IHadesInvoker
    {
        IAsyncEnumerable<Envelope> CallAsync(
            Envelope request, 
            CancellationToken cancellationToken = default);
    }
}