using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Handlers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal
{
    internal class ClientStreamingHandler : IClientStreamingHandler
    {
        private readonly Func<IAsyncEnumerable<Envelope>, CancellationToken, Task<Envelope>> _invoker;

        public ClientStreamingHandler(Func<IAsyncEnumerable<Envelope>, CancellationToken, Task<Envelope>> invoker)
        {
            _invoker = invoker
                ?? throw new ArgumentNullException(nameof(invoker));
        }

        public async Task<Envelope> CallAsync(IAsyncEnumerable<Envelope> stream, CancellationToken cancellationToken = default)
        {
            return await _invoker.Invoke(stream, cancellationToken);
        }
    }
}
