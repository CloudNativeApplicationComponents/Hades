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
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Hades.Transport.Grpc.Internal.Client.EndpointAccessors
{
    internal class GrpcUnaryClientEndpointAccessor :
        GrpcClientEndpointAccessorBase<UnaryClientWrapper, Envelope, Envelope>,
        ISingleEndpointWriter,
        ICorrelativeSingleEndpointReader,
        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<Envelope>> _callbackInbox;
        private readonly ITargetBlock<IncommingRequest> _block;

        public GrpcUnaryClientEndpointAccessor(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
            : base(serializer, deserializer, grpcMethodFactory, options)
        {

            _ = options
                ?? throw new ArgumentNullException(nameof(options));

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

        protected override MethodType MethodType => MethodType.Unary;

        protected override UnaryClientWrapper CreateClient(GrpcChannel channel, Method<ProtobufMessage, ProtobufMessage> method)
        {
            return new UnaryClientWrapper(channel, method);
        }

        ISingleEndpointWriter IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>.Writer => this;

        ICorrelativeSingleEndpointReader IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>.Reader => this;

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
            //TODO Log and exception handling
            var request = incommingRequest.Request;
            var (message, metadata) = Serializer.Serialize(request);
            var callOptions = new CallOptions(cancellationToken: CancellationTokenSource.Token)
                .WithHeaders(metadata);
            var result = Client.Send(message, callOptions);

            var response = await Deserializer.Deserialize(result);
            return new OutgoingResponse(response, incommingRequest.Correlation);
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
            Envelope request,
            CancellationToken cancellationToken = default)
        {
            _ = request
                ?? throw new ArgumentNullException(nameof(request));

            CorrelativeSubmission correlation = new CorrelativeSubmission();
            var incommingRequest = new IncommingRequest(request, correlation);

            _ = _callbackInbox.GetOrAdd(correlation.Id, _ => new TaskCompletionSource<Envelope>());

            // TODO raise error on post false
            var succ = _block.Post(incommingRequest);

            if (!succ)
            {
                _ = _callbackInbox.TryRemove(correlation.Id, out var ts);
                // TODO raise error on remove false
                ts.SetException(new Exception("Worker is completed and could not process any more envelopes."));
            }

            return await Task.FromResult(correlation);
        }
    }
}
