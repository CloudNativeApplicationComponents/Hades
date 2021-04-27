using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcEventServerEndpointAccessor :
        GrpcServerEndpointAccessorBase,
        ISingleEndpointObservable,
        IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter>,
        IDisposable
    {
        private readonly List<Subscription> _subscriptions;

        public GrpcEventServerEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
            : base(serializer, deserializer)
        {
            _subscriptions = new List<Subscription>();
        }

        NopEndpointWriter IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter>.Writer => NopEndpointWriter.Nop;
        ISingleEndpointObservable IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter>.Observable => this;

        public IDisposable Subscribe(ISingleEndpointObserver endpointObserver)
        {
            _ = endpointObserver
                ?? throw new ArgumentNullException(nameof(endpointObserver));
            var subscription = new Subscription(_subscriptions, endpointObserver);
            return subscription;
        }

        protected override void Dispose(bool disposing)
        {
            _subscriptions.Clear();
            base.Dispose(disposing);
        }

        public async Task<Empty> CallAsync(ProtobufMessage request, ServerCallContext context)
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
            return new Empty();
        }

        private class CorrelativeSubmission : CorrelativeSubmissionBase
        {
            public CorrelativeSubmission()
            {
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
