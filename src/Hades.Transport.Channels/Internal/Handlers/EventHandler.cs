using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal
{
    internal class EventHandler : IEventHandler
    {
        private readonly Func<Envelope, CancellationToken, Task> _invoker;

        public EventHandler(Func<Envelope, CancellationToken, Task> invoker)
        {
            _invoker = invoker
                ?? throw new ArgumentNullException(nameof(invoker));
        }

        public async Task PublishAsync(Envelope envelope, CancellationToken cancellationToken = default)
        {
            await _invoker.Invoke(envelope, cancellationToken);
        }
    }
}
