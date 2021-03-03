using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Client.Channels
{
    internal class DuplexStreamingClientChannel :
        ClientChannelBase<IDuplexStreamingInvoker, ICorrelativeStreamEndpointReader, IStreamEndpointWriter>,
        IDuplexStreamingClientChannel
    {
        public DuplexStreamingClientChannel(
            IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter> endpointAccessor)
            : base(endpointAccessor)
        {
        }

        protected virtual async IAsyncEnumerable<Envelope> CallAsync(
            IAsyncEnumerable<Envelope> envelopeStream,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
                // Set same CorrelationId for all items of stream, first envelope's correlationId has been selected if it is filled
                envelope.CorrelationId = correlationId;
            }

            var correlation = await EndpointAccessor.Writer.WriteAsync(envelopeStream, cancellationToken);
            await foreach (var item in EndpointAccessor.Reader.ReadAsync(correlation).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public override IDuplexStreamingInvoker CreateInvoker()
        {
            return new DuplexStreamingInvoker(this);
        }

        private class DuplexStreamingInvoker : IDuplexStreamingInvoker
        {
            private DuplexStreamingClientChannel _instance;
            public DuplexStreamingInvoker(DuplexStreamingClientChannel instance)
            {
                _instance = instance;
            }

            public IAsyncEnumerable<Envelope> CallAsync(
                IAsyncEnumerable<Envelope> stream, 
                CancellationToken cancellationToken = default)
            {
                return _instance.CallAsync(stream, cancellationToken);
            }
        }
    }
}
