using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Invokers;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Channels.Abstraction.Interceptors
{
    public interface IDuplexStreamingInterceptor :
        IChannelInterceptor
    {
        IAsyncEnumerable<Envelope> CallAsync(
            IDuplexStreamingInvoker invoker,
            IAsyncEnumerable<Envelope> stream,
            CancellationToken cancellationToken = default);
    }
}