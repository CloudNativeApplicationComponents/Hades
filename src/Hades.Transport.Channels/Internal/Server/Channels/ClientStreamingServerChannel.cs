using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Server.Channels
{
    internal class ClientStreamingServerChannel :
        ServerChannelBase<IClientStreamingHandler, IClientStreamingInvoker, IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IStreamEndpointObserver,
        IClientStreamingServerChannel
    {
        private readonly IDisposable _subscription;

        public ClientStreamingServerChannel(
            IClientStreamingHandler handler,
            IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter> endpointAccessor)
            : base(handler, endpointAccessor)
        {
            _subscription = EndpointAccessor.Observable.Subscribe(this);
        }

        private async Task<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> stream,
            CancellationToken cancellationToken = default)
        {
            return await Handler.CallAsync(stream, cancellationToken);
        }

        async Task IStreamEndpointObserver.OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken)
        {
            _ = envelopeStream
                ?? throw new ArgumentNullException(nameof(envelopeStream));

            var response = await CallAsync(envelopeStream, cancellationToken);
            await EndpointAccessor.Writer.WriteAsync(correlativeSubmission, response);
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            base.Dispose(disposing);
        }
    }
}
