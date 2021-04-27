using Grpc.Core;
using Grpc.Net.Client;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Serialization;
using System;
using System.Threading;

namespace Hades.Transport.Grpc.Internal.Client.EndpointAccessors
{
    internal abstract class GrpcClientEndpointAccessorBase<TClientWrapper, TRequest, TResponse> : IDisposable
        where TClientWrapper : ClientBase<TClientWrapper>
    {
        protected IProtobufEnvelopeSerializer Serializer { get; }
        protected IProtobufEnvelopeDeserializer Deserializer { get; }
        protected CancellationTokenSource CancellationTokenSource { get; }
        protected TClientWrapper Client { get; }

        public GrpcClientEndpointAccessorBase(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
        {
            Serializer = serializer ??
                throw new ArgumentNullException(nameof(serializer));
            Deserializer = deserializer ??
                throw new ArgumentNullException(nameof(deserializer));
            _ = options
                ?? throw new ArgumentNullException(nameof(options));
            _ = grpcMethodFactory
                ?? throw new ArgumentNullException(nameof(grpcMethodFactory));

            CancellationTokenSource = new CancellationTokenSource();

            var method = grpcMethodFactory.Create<ProtobufMessage, ProtobufMessage>(
                options.ServiceName!,
                options.MethodName!,
                MethodType);

            var channelOptions = options.ToGrpcChannelOptions();
            using var channel = GrpcChannel.ForAddress(options.BaseUri!, channelOptions);
            Client = CreateClient(channel, method);
        }

        protected abstract MethodType MethodType { get; }

        protected abstract TClientWrapper CreateClient(
            GrpcChannel channel, 
            Method<ProtobufMessage, ProtobufMessage> method);

        void IDisposable.Dispose()
        {
            CancellationTokenSource.Cancel();
            GC.SuppressFinalize(this);
        }


        protected class CorrelativeSubmission : 
            CorrelativeSubmissionBase
        {
            public CorrelativeSubmission(string? id = null)
                : base(id)
            {
            }
        }

        protected class IncommingRequest
        {
            public TRequest Request { get; }
            public CorrelativeSubmissionBase Correlation { get; }
            public IncommingRequest(TRequest request, CorrelativeSubmissionBase correlation)
            {
                Request = request;
                Correlation = correlation;
            }
        }
        protected class OutgoingResponse
        {
            public TResponse Response { get; }
            public CorrelativeSubmissionBase Correlation { get; }
            public OutgoingResponse(TResponse response, CorrelativeSubmissionBase correlation)
            {
                Response = response;
                Correlation = correlation;
            }
        }
    }
}
