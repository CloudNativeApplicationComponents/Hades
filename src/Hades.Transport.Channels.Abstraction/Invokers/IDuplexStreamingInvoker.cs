using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Channels.Abstraction.Invokers
{
    public interface IDuplexStreamingInvoker :
        IHadesInvoker
    {
        IAsyncEnumerable<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> stream, 
            CancellationToken cancellationToken = default);
    }
}