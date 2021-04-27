using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Handlers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal
{
    internal class ServerStreamingHandler : IServerStreamingHandler
    {
        private readonly Func<Envelope, CancellationToken, IAsyncEnumerable<Envelope>> _invoker;

        public ServerStreamingHandler(Func<Envelope, CancellationToken, IAsyncEnumerable<Envelope>> invoker)
        {
            _invoker = invoker
                ?? throw new ArgumentNullException(nameof(invoker));
        }

        public async IAsyncEnumerable<Envelope> CallAsync(
            Envelope request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in _invoker.Invoke(request, cancellationToken).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }
    }
}
