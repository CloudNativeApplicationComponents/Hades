using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Server.Channels
{
    internal class DuplexStreamingServerChannel :
        ServerChannelBase<IDuplexStreamingHandler, IDuplexStreamingInvoker, IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IStreamEndpointObserver,
        IDuplexStreamingServerChannel
    {
        private readonly IDisposable _subscription;

        public DuplexStreamingServerChannel(
            IDuplexStreamingHandler handler,
            IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter> endpointAccessor)
            : base(handler, endpointAccessor)
        {
            _subscription = EndpointAccessor.Observable.Subscribe(this);
        }

        private async IAsyncEnumerable<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in Handler.CallAsync(request, cancellationToken).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        async Task IStreamEndpointObserver.OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken)
        {
            _ = envelopeStream
                ?? throw new ArgumentNullException(nameof(envelopeStream));

            var responseStream = CallAsync(envelopeStream, cancellationToken);
            await EndpointAccessor.Writer.WriteAsync(correlativeSubmission, responseStream);
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            base.Dispose(disposing);
        }
    }
}
