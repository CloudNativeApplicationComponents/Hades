using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Client.Channels
{
    internal class ClientStreamingClientChannel :
        ClientChannelBase<IClientStreamingInvoker, ICorrelativeSingleEndpointReader, IStreamEndpointWriter>,
        IClientStreamingClientChannel
    {
        public ClientStreamingClientChannel(
            IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter> endpointAccessor)
            : base(endpointAccessor)
        {
        }

        protected virtual async Task<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken = default)
        {
            _ = envelopeStream
                ?? throw new ArgumentNullException(nameof(envelopeStream));

            string correlationId = "";
            await foreach (var envelope in envelopeStream)
            {
                if (string.IsNullOrWhiteSpace(correlationId))
                {
                    if (string.IsNullOrWhiteSpace(envelope.CorrelationId))
                    {
                        correlationId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        correlationId = envelope.CorrelationId;
                    }
                }
                envelope.CorrelationId = correlationId;
            }
            var correlation = await EndpointAccessor.Writer.WriteAsync(envelopeStream, cancellationToken);
            return await EndpointAccessor.Reader.ReadAsync(correlation, cancellationToken);
        }

        public override IClientStreamingInvoker CreateInvoker()
        {
            return new ClientStreamingInvoker(this);
        }

        private class ClientStreamingInvoker : IClientStreamingInvoker
        {
            private ClientStreamingClientChannel _instance;
            public ClientStreamingInvoker(ClientStreamingClientChannel instance)
            {
                _instance = instance;
            }

            public async Task<Envelope> CallAsync(
                IAsyncEnumerable<Envelope> stream, 
                CancellationToken cancellationToken = default)
            {
                return await _instance.CallAsync(stream, cancellationToken);
            }
        }
    }
}
