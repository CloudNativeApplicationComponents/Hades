using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Client.Channels
{
    internal class EventClientChannel :
        ClientChannelBase<IEventInvoker, NopEndpointReader, ISingleEndpointWriter>,
        IEventClientChannel
    {
        public EventClientChannel(IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter> endpointAccessor)
            : base(endpointAccessor)
        {
        }

        protected virtual async Task PublishAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            await EndpointAccessor.Writer.WriteAsync(envelope, cancellationToken);
        }

        public override IEventInvoker CreateInvoker()
        {
            return new EventClientInvoker(this);
        }

        private class EventClientInvoker : IEventInvoker
        {
            private EventClientChannel _instance;
            public EventClientInvoker(EventClientChannel instance)
            {
                _instance = instance;
            }

            public async Task PublishAsync(Envelope envelope, CancellationToken cancellationToken = default)
            {
                await _instance.PublishAsync(envelope, cancellationToken);
            }
        }
    }
}
