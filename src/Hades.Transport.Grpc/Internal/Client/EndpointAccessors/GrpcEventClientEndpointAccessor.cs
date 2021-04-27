using Grpc.Core;
using Grpc.Net.Client;
using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Hades.Transport.Grpc.Internal.Client.EndpointAccessors
{
    internal class GrpcEventClientEndpointAccessor :
        GrpcClientEndpointAccessorBase<EventClientWrapper, Envelope, Envelope>,
        ISingleEndpointWriter,
        IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter>,
        IDisposable
    {
        private readonly ITargetBlock<IncommingRequest> _block;

        public GrpcEventClientEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
            : base(serializer, deserializer, grpcMethodFactory, options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var caller = new ActionBlock<IncommingRequest>(Call);
            _block = caller;
            CancellationTokenSource.Token.Register(() =>
            {
                caller.Complete();
            });
        }

        protected override MethodType MethodType => MethodType.Unary;

        protected override EventClientWrapper CreateClient(GrpcChannel channel, Method<ProtobufMessage, ProtobufMessage> method)
        {
            return new EventClientWrapper(channel, method);
        }

        ISingleEndpointWriter IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter>.Writer => this;

        NopEndpointReader IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter>.Reader => NopEndpointReader.Nop;

        private async Task Call(IncommingRequest incommingRequest)
        {
            //TODO Log and exception handling
            var (message, metadata) = Serializer.Serialize(incommingRequest.Request);
            var callOptions = new CallOptions(cancellationToken: CancellationTokenSource.Token)
                .WithHeaders(metadata);
            var result = Client.Send(message, callOptions);
            await result.ResponseAsync;
        }

        public async Task<ICorrelativeSubmission> WriteAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));

            var correlation = new CorrelativeSubmission();
            // TODO raise error on post false
            _block.Post(new IncommingRequest(envelope, correlation));

            return await Task.FromResult(correlation);
        }
    }
}
