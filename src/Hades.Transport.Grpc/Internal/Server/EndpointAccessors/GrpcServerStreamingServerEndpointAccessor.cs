using Hades.Transport.Abstraction.Server;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using tc = System.Threading.Channels;
using System.Linq;
using CloudNativeApplicationComponents.Utils;
using Hades.Transport.Grpc.Internal.Serialization;

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcServerStreamingServerEndpointAccessor :
        GrpcServerEndpointAccessorBase,
        ISingleEndpointObservable,
        ICorrelativeStreamEndpointWriter,
        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IDisposable
    {
        private readonly List<Subscription> _subscriptions;

        public GrpcServerStreamingServerEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
            : base(serializer, deserializer)
        {
            _subscriptions = new List<Subscription>();
        }

        ICorrelativeStreamEndpointWriter IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>.Writer => this;

        ISingleEndpointObservable IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>.Observable => this;

        public IDisposable Subscribe(ISingleEndpointObserver endpointObserver)
        {
            _ = endpointObserver
                ?? throw new ArgumentNullException(nameof(endpointObserver));
            var subscription = new Subscription(_subscriptions, endpointObserver);
            return subscription;
        }

        public async Task WriteAsync(ICorrelativeSubmission correlativeSubmission, IAsyncEnumerable<Envelope> envelopeStream, CancellationToken cancellationToken = default)
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

        public async Task CallAsync(ProtobufMessage request, IServerStreamWriter<ProtobufMessage> responseStream, ServerCallContext context)
        {
            if (!_subscriptions.Any())
            {
                throw new NotSupportedException("There is no handler to handle request.");
            }
            var correlation = new CorrelativeSubmission();
            var envelope = Deserializer.Deserialize(request, context.RequestHeaders);

            //TODO more than one subscription :(
            var subscription = _subscriptions.First();
            await subscription.EndpointObserver.OnReadAsync(correlation,
                envelope,
                context.CancellationToken);
            var trailerAdd = false;
            await foreach (var item in correlation.Channel.Reader.ReadAllAsync().WithCancellation(context.CancellationToken))
            {
                //TODO the metadata is left here!
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
            public ISingleEndpointObserver EndpointObserver { get; }
            public Subscription(IList<Subscription> subscriptions, ISingleEndpointObserver endpointObserver)
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
