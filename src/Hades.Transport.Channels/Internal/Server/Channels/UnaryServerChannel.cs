using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Server.Channels
{
    internal class UnaryServerChannel :
        ServerChannelBase<IUnaryHandler, IUnaryInvoker, ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>,
        ISingleEndpointObserver,
        IUnaryServerChannel
    {
        private readonly IDisposable _subscription;

        public UnaryServerChannel(
            IUnaryHandler handler,
            IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter> endpointAccessor)
            : base(handler, endpointAccessor)
        {
            _subscription = EndpointAccessor.Observable.Subscribe(this);
        }

        private async Task<Envelope> CallAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            return await Handler.CallAsync(envelope, cancellationToken);
        }

        async Task ISingleEndpointObserver.OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            Envelope envelope,
            CancellationToken cancellationToken)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            var response = await CallAsync(envelope, cancellationToken);
            await EndpointAccessor.Writer.WriteAsync(correlativeSubmission, response);
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            base.Dispose(disposing);
        }
    }
}
