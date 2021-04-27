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
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Hades.Transport.Grpc.Internal.Client.EndpointAccessors
{
    internal class GrpcClientStreamingClientEndpointAccessor :
        GrpcClientEndpointAccessorBase<ClientStreamingClientWrapper, IAsyncEnumerable<Envelope>, Envelope>,
        ICorrelativeSingleEndpointReader,
        IStreamEndpointWriter,
        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>,
        IDisposable
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<Envelope>> _callbackInbox;
        private readonly ITargetBlock<IncommingRequest> _block;

        public GrpcClientStreamingClientEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
            : base(serializer, deserializer, grpcMethodFactory, options)
        {

            _callbackInbox = new ConcurrentDictionary<string, TaskCompletionSource<Envelope>>();

            var caller = new TransformBlock<IncommingRequest, OutgoingResponse>(Call);
            _block = caller;
            var sendResponse = new ActionBlock<OutgoingResponse>(SendToInbox);
            var flow = caller.LinkTo(sendResponse, new DataflowLinkOptions { PropagateCompletion = true });
            CancellationTokenSource.Token.Register(() =>
            {
                caller.Complete();
                flow.Dispose();
            });
        }

        protected override MethodType MethodType => MethodType.ClientStreaming;

        protected override ClientStreamingClientWrapper CreateClient(GrpcChannel channel, Method<ProtobufMessage, ProtobufMessage> method)
        {
            return new ClientStreamingClientWrapper(channel, method);
        }

        IStreamEndpointWriter IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>.Writer => this;

        ICorrelativeSingleEndpointReader IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>.Reader => this;


        private async Task SendToInbox(OutgoingResponse outgoingResponse)
        {
            var correlationId = outgoingResponse.Correlation.Id;
            var response = outgoingResponse.Response;
            if (_callbackInbox.TryRemove(correlationId, out var taskCompletionSource))
            {
                taskCompletionSource.SetResult(response);
                await Task.CompletedTask;
            }
            else
            {
                //TODO specific exception and log
                throw new NotSupportedException($"Could not find any inbox for {correlationId}.");
            }
        }

        private async Task<OutgoingResponse> Call(IncommingRequest incommingRequest)
        {
            AsyncClientStreamingCall<ProtobufMessage, ProtobufMessage> streamingCall = default!;
            bool isFirstEnvelope = true;

            await foreach (var item in incommingRequest.Request)
            {
                var (request, metadata) = Serializer.Serialize(item);

                if (isFirstEnvelope)
                {
                    streamingCall = Client.Send(
                        new CallOptions(cancellationToken: CancellationTokenSource.Token)
                        .WithHeaders(metadata));

                    isFirstEnvelope = false;
                }

                await streamingCall.RequestStream.WriteAsync(request);
            }

            var response = await Deserializer.Deserialize(streamingCall!);
            return await Task.FromResult(new OutgoingResponse(response, incommingRequest.Correlation));
        }

        public async Task<Envelope> ReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            CancellationToken cancellationToken = default)
        {
            _ = correlativeSubmission
                ?? throw new ArgumentNullException(nameof(correlativeSubmission));

            if (correlativeSubmission is CorrelativeSubmission correlation)
            {
                if (_callbackInbox.TryGetValue(correlation.Id, out var taskCompletionSource))
                {
                    return await taskCompletionSource.Task;
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
            IAsyncEnumerable<Envelope> requestStream,
            CancellationToken cancellationToken = default)
        {
            _ = requestStream
                ?? throw new ArgumentNullException(nameof(requestStream));

            CorrelativeSubmission? correlation = new CorrelativeSubmission();
            _ = _callbackInbox.GetOrAdd(correlation.Id, _ => new TaskCompletionSource<Envelope>());

            // TODO raise error on post false
            var succ = _block.Post(new IncommingRequest(requestStream, correlation));

            if (correlation is not null && !succ)
            {
                _ = _callbackInbox.TryRemove(correlation!.Id, out var _);
            }
            return await Task.FromResult(correlation ?? new CorrelativeSubmission());
        }
    }
}
