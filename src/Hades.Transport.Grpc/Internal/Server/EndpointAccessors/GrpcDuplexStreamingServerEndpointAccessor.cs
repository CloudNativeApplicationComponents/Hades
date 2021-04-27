using CloudNativeApplicationComponents.Utils;
using Grpc.Core;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tc = System.Threading.Channels;

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcDuplexStreamingServerEndpointAccessor :
        GrpcServerEndpointAccessorBase,
        IStreamEndpointObservable,
        ICorrelativeStreamEndpointWriter,
        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IDisposable
    {
        private readonly List<Subscription> _subscriptions;

        public GrpcDuplexStreamingServerEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
            : base(serializer, deserializer)
        {
            _subscriptions = new List<Subscription>();
        }

        ICorrelativeStreamEndpointWriter IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>.Writer => this;
        IStreamEndpointObservable IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>.Observable => this;

        public IDisposable Subscribe(IStreamEndpointObserver endpointObserver)
        {
            _ = endpointObserver
                ?? throw new ArgumentNullException(nameof(endpointObserver));
            var subscription = new Subscription(_subscriptions, endpointObserver);
            return subscription;
        }

        public async Task WriteAsync(
            ICorrelativeSubmission correlativeSubmission,
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken = default)
        {
            if (correlativeSubmission is CorrelativeSubmission correlation)
            {
                await foreach (var item in envelopeStream)
                {
                    await correlation.Channel.Writer.WriteAsync(item);
                }
            }
            else
            {
                throw new ArgumentException("The correlativeSubmission is not supported.", nameof(correlativeSubmission));
            }
        }

        public async Task CallAsync(
            IAsyncStreamReader<ProtobufMessage> requestStream,
            IServerStreamWriter<ProtobufMessage> responseStream,
            ServerCallContext context)
        {
            if (!_subscriptions.Any())
            {
                throw new NotSupportedException("There is no handler to handle request.");
            }
            var correlation = new CorrelativeSubmission();
            var envelopes = Deserializer.Deserialize(requestStream, context.RequestHeaders);

            //TODO more than one subscription :(
            var subscription = _subscriptions.First();
            await subscription.EndpointObserver.OnReadAsync(correlation,
                envelopes,
                context.CancellationToken);

            var trailerAdd = false;
            await foreach (var item in correlation.Channel.Reader.ReadAllAsync().WithCancellation(context.CancellationToken))
            {
                //TODO metadata is not set here ! :(
                var (message, metadata) = Serializer.Serialize(item);
                if (!trailerAdd)
                {
                    metadata.ForEach(context.ResponseTrailers.Add);
                    trailerAdd = true;
                }
                await responseStream.WriteAsync(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _subscriptions.Clear();
            base.Dispose(disposing);
        }

        private class CorrelativeSubmission : CorrelativeSubmissionBase
        {
            public tc::Channel<Envelope> Channel { get; }
            public CorrelativeSubmission()
            {
                Channel = tc::Channel.CreateUnbounded<Envelope>();
            }
            protected override void Dispose(bool disposing)
            {
                Channel.Writer.Complete();
                base.Dispose(disposing);
            }
        }

        private class Subscription : IDisposable
        {
            private readonly IList<Subscription> _subscriptions;
            public IStreamEndpointObserver EndpointObserver { get; }
            public Subscription(IList<Subscription> subscriptions, IStreamEndpointObserver endpointObserver)
            {
                _subscriptions = subscriptions;
                EndpointObserver = endpointObserver;
                subscriptions.Add(this);
            }
            public void Dispose()
            {
                _subscriptions.Remove(this);
                GC.SuppressFinalize(this);
            }
        }
    }
}
