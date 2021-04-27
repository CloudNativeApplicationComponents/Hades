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
    internal class UnaryClientChannel :
        ClientChannelBase<IUnaryInvoker, ICorrelativeSingleEndpointReader, ISingleEndpointWriter>,
        IUnaryClientChannel
    {
        public UnaryClientChannel(
            IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter> endpointAccessor)
            : base(endpointAccessor)
        {
        }

        protected virtual async Task<Envelope> CallAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            if (string.IsNullOrWhiteSpace(envelope.CorrelationId))
            {
                envelope.CorrelationId = Guid.NewGuid().ToString();
            }
            var correlation = await EndpointAccessor.Writer.WriteAsync(envelope, cancellationToken);
            return await EndpointAccessor.Reader.ReadAsync(correlation, cancellationToken);
        }


        public override IUnaryInvoker CreateInvoker()
        {
            return new UnaryInvoker(this);
        }

        private class UnaryInvoker : IUnaryInvoker
        {
            private UnaryClientChannel _instance;
            public UnaryInvoker(UnaryClientChannel instance)
            {
                _instance = instance;
            }

            public async Task<Envelope> CallAsync(
                Envelope request, 
                CancellationToken cancellationToken = default)
            {
                return await _instance.CallAsync(request, cancellationToken);
            }
        }
    }
}
