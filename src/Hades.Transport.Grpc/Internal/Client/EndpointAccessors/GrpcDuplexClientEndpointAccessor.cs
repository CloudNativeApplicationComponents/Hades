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
    internal class GrpcDuplexStreamingClientEndpointAccessor :
        GrpcClientEndpointAccessorBase<DuplexStreamingClientWrapper, IAsyncEnumerable<Envelope>, IAsyncEnumerable<Envelope>>,
        IStreamEndpointWriter,
        ICorrelativeStreamEndpointReader,
        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>,
        IDisposable
    {
        private readonly ConcurrentDictionary<string, tc.Channel<Envelope>> _callbackInbox;
        private readonly ITargetBlock<IncommingRequest> _block;

        IStreamEndpointWriter IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>.Writer => this;
        ICorrelativeStreamEndpointReader IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>.Reader => this;


        public GrpcDuplexStreamingClientEndpointAccessor(
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
            _block = caller;

            var sendResponse = new ActionBlock<OutgoingResponse>(SendToInbox);
            var flow = caller.LinkTo(sendResponse, new DataflowLinkOptions { PropagateCompletion = true });
            CancellationTokenSource.Token.Register(() =>
            {
                caller.Complete();
                flow.Dispose();
            });
        }

        protected override MethodType MethodType => MethodType.DuplexStreaming;

        protected override DuplexStreamingClientWrapper CreateClient(GrpcChannel channel, Method<ProtobufMessage, ProtobufMessage> method)
        {
            return new DuplexStreamingClientWrapper(channel, method);
        }

        private async Task<OutgoingResponse> Call(IncommingRequest incommingRequest)
        {
            AsyncDuplexStreamingCall<ProtobufMessage, ProtobufMessage> callStream = default!;
            bool isFirstEnvelop = true;

            await foreach (var item in incommingRequest.Request)
            {
                var (message, metadata) = Serializer.Serialize(item);

                if (isFirstEnvelop)
                {
                    callStream = Client.Send(new CallOptions(cancellationToken: CancellationTokenSource.Token).WithHeaders(metadata));
                    isFirstEnvelop = false;
                }
                await callStream.RequestStream.WriteAsync(message);
            }

            //TODO here we need try catch to handle rpc exception!!
            var responses = Deserializer.Deserialize(callStream);
            return new OutgoingResponse(responses, incommingRequest.Correlation);
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
            IAsyncEnumerable<Envelope> requestStream,
            CancellationToken cancellationToken = default)
        {
            _ = requestStream
                ?? throw new ArgumentNullException(nameof(requestStream));

            CorrelativeSubmission? correlation = new();
            _ = _callbackInbox.GetOrAdd(correlation.Id, _ => tc.Channel.CreateUnbounded<Envelope>());

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
