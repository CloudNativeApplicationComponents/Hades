using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Internal.Channels
{
    internal class UnaryHandler : IUnaryHandler
    {
        private readonly Func<Envelope, CancellationToken, Task<Envelope>> _invoker;

        public UnaryHandler(Func<Envelope, CancellationToken, Task<Envelope>> invoker)
        {
            _invoker = invoker
                ?? throw new ArgumentNullException(nameof(invoker));
        }

        public async Task<Envelope> CallAsync(Envelope request, CancellationToken cancellationToken = default)
        {
            return await _invoker.Invoke(request, cancellationToken);
        }
    }
}
