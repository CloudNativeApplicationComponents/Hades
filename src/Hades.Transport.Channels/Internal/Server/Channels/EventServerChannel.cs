using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Server.Channels
{
    internal class EventServerChannel :
        ServerChannelBase<IEventHandler, IEventInvoker, ISingleEndpointObservable, NopEndpointWriter>,
        ISingleEndpointObserver,
        IEventServerChannel
    {
        private readonly IDisposable _subscription;

        public EventServerChannel(
            IEventHandler handler,
            IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter> endpointAccessor)
            : base(handler, endpointAccessor)
        {
            _subscription = EndpointAccessor.Observable.Subscribe(this);
        }

        private async Task PublishAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            await Handler.PublishAsync(envelope, cancellationToken);
        }

        async Task ISingleEndpointObserver.OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            Envelope envelope,
            CancellationToken cancellationToken)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            await PublishAsync(envelope, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            base.Dispose(disposing);
        }
    }
}
