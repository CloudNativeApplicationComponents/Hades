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
    internal class ServerStreamingServerChannel :
        ServerChannelBase<IServerStreamingHandler, IServerStreamingInvoker, ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>,
        ISingleEndpointObserver,
        IServerStreamingServerChannel
    {
        private readonly IDisposable _subscription;

        public ServerStreamingServerChannel(
            IServerStreamingHandler handler,
            IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter> endpointAccessor)
            : base(handler, endpointAccessor)
        {
            _subscription = EndpointAccessor.Observable.Subscribe(this);
        }

        private async IAsyncEnumerable<Envelope> CallAsync(
            Envelope envelope,
          [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in Handler.CallAsync(envelope, cancellationToken).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        async Task ISingleEndpointObserver.OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            Envelope envelope,
            CancellationToken cancellationToken)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            var responseStream = CallAsync(envelope, cancellationToken);
            await EndpointAccessor.Writer.WriteAsync(correlativeSubmission, responseStream);
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            base.Dispose(disposing);
        }
    }
}
