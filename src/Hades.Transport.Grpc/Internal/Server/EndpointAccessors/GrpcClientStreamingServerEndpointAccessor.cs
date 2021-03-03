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

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcClientStreamingServerEndpointAccessor :
        GrpcServerEndpointAccessorBase,
        IStreamEndpointObservable,
        ICorrelativeSingleEndpointWriter,
        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IDisposable
    {
        private readonly List<Subscription> _subscriptions;

        public GrpcClientStreamingServerEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
            : base(serializer, deserializer)
        {
            _subscriptions = new List<Subscription>();
        }

        ICorrelativeSingleEndpointWriter IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>.Writer => this;
        IStreamEndpointObservable IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>.Observable => this;

        public IDisposable Subscribe(IStreamEndpointObserver endpointObserver)
        {
            _ = endpointObserver
                ?? throw new ArgumentNullException(nameof(endpointObserver));
            var subscription = new Subscription(_subscriptions, endpointObserver);
            return subscription;
        }

        public async Task WriteAsync(ICorrelativeSubmission correlativeSubmission, Envelope envelope, CancellationToken cancellationToken = default)
        {
            if (correlativeSubmission is CorrelativeSubmission correlation)
            {
                correlation.TaskCompletion.SetResult(envelope);
                await Task.CompletedTask;
            }
            else
            {
                throw new ArgumentException("The correlativeSubmission is not supported.", nameof(correlativeSubmission));
            }
        }

        public async Task<ProtobufMessage> CallAsync(
            IAsyncStreamReader<ProtobufMessage> requestStream, 
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
            var res = await correlation.TaskCompletion.Task;

            //TODO Shouldn't we return the medatadata here?
            var (message, metadata) = Serializer.Serialize(res);
            metadata.ForEach(context.ResponseTrailers.Add);
            return message;
        }

        protected override void Dispose(bool disposing)
        {
            _subscriptions.Clear();
            base.Dispose(disposing);
        }

        private class CorrelativeSubmission : CorrelativeSubmissionBase
        {
            public TaskCompletionSource<Envelope> TaskCompletion { get; }
            public CorrelativeSubmission()
            {
                TaskCompletion = new TaskCompletionSource<Envelope>();
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
