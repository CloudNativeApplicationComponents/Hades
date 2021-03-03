using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Invokers;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Channels.Abstraction.Interceptors
{
    public interface IServerStreamingInterceptor :
        IChannelInterceptor
    {
        IAsyncEnumerable<Envelope> CallAsync(
            IServerStreamingInvoker invoker,
            Envelope request,
            CancellationToken cancellationToken = default);
    }
}