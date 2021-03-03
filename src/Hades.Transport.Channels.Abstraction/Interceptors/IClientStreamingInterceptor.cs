using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Invokers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Interceptors
{
    public interface IClientStreamingInterceptor :
        IChannelInterceptor
    {
        Task<Envelope> CallAsync(
            IClientStreamingInvoker invoker,
            IAsyncEnumerable<Envelope> stream,
            CancellationToken cancellationToken = default);
    }
}