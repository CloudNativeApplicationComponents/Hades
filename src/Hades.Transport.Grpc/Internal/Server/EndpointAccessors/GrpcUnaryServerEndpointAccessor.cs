using Grpc.Core;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Internal.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CloudNativeApplicationComponents.Utils;
using Hades.Transport.Grpc.Internal.Serialization;

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcUnaryServerEndpointAccessor :
        GrpcServerEndpointAccessorBase,
        ISingleEndpointObservable,
        ICorrelativeSingleEndpointWriter,
        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IDisposable
    {
        private readonly List<Subscription> _subscriptions;

        public GrpcUnaryServerEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
            : base(serializer, deserializer)
        {
            _subscriptions = new List<Subscription>();
        }

        ICorrelativeSingleEndpointWriter IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>.Writer => this;
        ISingleEndpointObservable IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>.Observable => this;

        void IDisposable.Dispose()
        {
            _subscriptions.Clear();
            GC.SuppressFinalize(this);
        }

        public IDisposable Subscribe(ISingleEndpointObserver endpointObserver)
        {
            _ = endpointObserver
                ?? throw new ArgumentNullException(nameof(endpointObserver));
            var subscription = new Subscription(_subscriptions, endpointObserver);
            return subscription;
        }

        public async Task WriteAsync(
            ICorrelativeSubmission correlativeSubmission,
            Envelope envelope,
            CancellationToken cancellationToken = default)
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

        public async Task<ProtobufMessage> CallAsync(ProtobufMessage request, ServerCallContext context)
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
            var res = await correlation.TaskCompletion.Task;

            //TODO the metadata is left here!
            var (message, metadata) = Serializer.Serialize(res);
            metadata.ForEach(context.ResponseTrailers.Add);
            return message;
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
