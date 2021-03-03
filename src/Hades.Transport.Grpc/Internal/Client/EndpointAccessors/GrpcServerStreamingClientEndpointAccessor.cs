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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using tc = System.Threading.Channels;

namespace Hades.Transport.Grpc.Internal.Client.EndpointAccessors
{
    internal class GrpcServerStreamingClientEndpointAccessor :
        GrpcClientEndpointAccessorBase<ServerStreamingClientWrapper, Envelope, IAsyncEnumerable<Envelope>>,
        ISingleEndpointWriter,
        ICorrelativeStreamEndpointReader,
        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>,
        IDisposable
    {
        private readonly ConcurrentDictionary<string, tc.Channel<Envelope>> _callbackInbox;
        private readonly ITargetBlock<IncommingRequest> _block;

        ISingleEndpointWriter IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>.Writer => this;
        ICorrelativeStreamEndpointReader IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>.Reader => this;

        public GrpcServerStreamingClientEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
            : base(serializer, deserializer, grpcMethodFactory, options)
        {

            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            _callbackInbox = new ConcurrentDictionary<string, tc.Channel<Envelope>>();

            var caller = new TransformBlock<IncommingRequest, OutgoingResponse>(Call);
            _block = caller!;

            var sendResponse = new ActionBlock<OutgoingResponse>(SendToInbox);
            var flow = caller.LinkTo(sendResponse, new DataflowLinkOptions { PropagateCompletion = true });
            CancellationTokenSource.Token.Register(() =>
            {
                caller.Complete();
                flow.Dispose();
            });

        }

        protected override MethodType MethodType => MethodType.ServerStreaming;

        protected override ServerStreamingClientWrapper CreateClient(GrpcChannel channel, Method<ProtobufMessage, ProtobufMessage> method)
        {
            return new ServerStreamingClientWrapper(channel, method);
        }

        private async Task SendToInbox(OutgoingResponse outgoingResponse)
        {
            var correlationId = outgoingResponse.Correlation.Id;
            var responses = outgoingResponse.Response;
            if (_callbackInbox.TryRemove(correlationId, out var channel))
            {
                await foreach (var response in responses)
                {
                    await channel.Writer.WriteAsync(response);
                }

                channel.Writer.Complete();
            }
            else
            {
                //TODO specific exception and log
                throw new NotSupportedException($"Could not find any inbox for {correlationId}.");
            }
        }

        private async Task<OutgoingResponse> Call(IncommingRequest incommingRequest)
        {
            //TODO Log and exception handling
            var (message, metadata) = Serializer.Serialize(incommingRequest.Request);

            var callOptions = new CallOptions(cancellationToken: CancellationTokenSource.Token)
                .WithHeaders(metadata);

            var serverStreamingCall = Client.Send(message, callOptions);

            var responses = Deserializer.Deserialize(serverStreamingCall);

            return await Task.FromResult(new OutgoingResponse(responses, incommingRequest.Correlation));
        }

        public async IAsyncEnumerable<Envelope> ReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _ = correlativeSubmission
               ?? throw new ArgumentNullException(nameof(correlativeSubmission));

            if (correlativeSubmission is CorrelativeSubmission correlation)
            {
                if (_callbackInbox.TryGetValue(correlation.Id, out var channel))
                {
                    await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken).WithCancellation(cancellationToken))
                    {
                        yield return item;
                    }
                }
                else
                {
                    //TODO specific exception and log
                    throw new NotSupportedException($"Could not find any inbox for {correlation.Id}.");
                }
            }

            //TODO specific exception and log
            throw new ArgumentException("The correlativeSubmission is not supported.");
        }

        public async Task<ICorrelativeSubmission> WriteAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default)
        {
            _ = envelope
              ?? throw new ArgumentNullException(nameof(envelope));

            //var correlation = new CorrelativeSubmission(envelope.CorrelationId);
            CorrelativeSubmission? correlation = new();
            _ = _callbackInbox.GetOrAdd(correlation.Id, tc.Channel.CreateUnbounded<Envelope>());

            var succ = _block.Post(new IncommingRequest(envelope, correlation));

            if (correlation is not null && !succ)
            {
                _ = _callbackInbox.TryRemove(correlation!.Id, out var _);
            }

            return await Task.FromResult(correlation ?? new CorrelativeSubmission());
        }
    }
}
