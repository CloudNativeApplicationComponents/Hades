using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Internal.Client.Channels
{
    internal class ServerStreamingClientChannel :
        ClientChannelBase<IServerStreamingInvoker, ICorrelativeStreamEndpointReader, ISingleEndpointWriter>,
        IServerStreamingClientChannel
    {
        public ServerStreamingClientChannel(
            IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter> endpointAccessor)
            : base(endpointAccessor)
        {
        }

        protected virtual async IAsyncEnumerable<Envelope> CallAsync(
            Envelope envelope,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            if (string.IsNullOrWhiteSpace(envelope.CorrelationId))
            {
                envelope.CorrelationId = Guid.NewGuid().ToString();
            }
            var correlation = await EndpointAccessor.Writer.WriteAsync(envelope, cancellationToken);
            await foreach (var item in EndpointAccessor.Reader.ReadAsync(correlation).WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }


        public override IServerStreamingInvoker CreateInvoker()
        {
            return new ServerStreamingInvoker(this);
        }

        private class ServerStreamingInvoker : IServerStreamingInvoker
        {
            private ServerStreamingClientChannel _instance;
            public ServerStreamingInvoker(ServerStreamingClientChannel instance)
            {
                _instance = instance;
            }

            public IAsyncEnumerable<Envelope> CallAsync(
                Envelope request, 
                CancellationToken cancellationToken = default)
            {
                return _instance.CallAsync(request, cancellationToken);
            }
        }
    }
}
