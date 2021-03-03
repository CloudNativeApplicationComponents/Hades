using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Handlers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal
{
    internal class DuplexStreamingHandler : IDuplexStreamingHandler
    {
        private readonly Func<IAsyncEnumerable<Envelope>, CancellationToken, IAsyncEnumerable<Envelope>> _invoker;

        public DuplexStreamingHandler(Func<IAsyncEnumerable<Envelope>, CancellationToken, IAsyncEnumerable<Envelope>> invoker)
        {
            _invoker = invoker
                ?? throw new ArgumentNullException(nameof(invoker));
        }

        public async IAsyncEnumerable<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> stream,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in _invoker.Invoke(stream, cancellationToken).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }
    }
}
