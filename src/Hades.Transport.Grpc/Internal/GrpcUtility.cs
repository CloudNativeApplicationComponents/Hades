using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Abstraction.Serialization;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Grpc.Internal
{
    internal static class GrpcUtility
    {
        public static Method<TRequest, TResponse> CreateMethod<TRequest, TResponse>(
            string serviceName,
            string methodName,
            MethodType methodType,
            IGenericProtobufSerializer serializer,
            IGenericProtobufDeserializer deserializer)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
        {
            var requestParser = new MessageParser<TRequest>(() => new TRequest()).WithDiscardUnknownFields(false);
            var responseParser = new MessageParser<TResponse>(() => new TResponse()).WithDiscardUnknownFields(false);
            var requestMarshaller = Marshallers.Create(serializer.SerializeMessage, context => deserializer.DeserializeMessage(context, requestParser));
            var responseMarshaller = Marshallers.Create(serializer.SerializeMessage, context => deserializer.DeserializeMessage(context, responseParser));

            return new Method<TRequest, TResponse>(methodType, serviceName, methodName, requestMarshaller, responseMarshaller);
        }

        public static async Task<Envelope> Deserialize(
            this IProtobufEnvelopeDeserializer protobufEnvelopeDeserializer,
            AsyncUnaryCall<ProtobufMessage> callStream,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var all = Task.WhenAll(callStream.ResponseAsync, callStream.ResponseHeadersAsync);
                await Task.WhenAny(all, Task.Delay(Timeout.Infinite, cancellationToken));

                cancellationToken.ThrowIfCancellationRequested();

                return protobufEnvelopeDeserializer.Deserialize(
                    await callStream.ResponseAsync,
                    await callStream.ResponseHeadersAsync);
            }
            catch (RpcException ex)
            {
                return protobufEnvelopeDeserializer.ErrorDeserializer.DeserializeException(ex);
            }
        }

        public static async Task<Envelope> Deserialize(
            this IProtobufEnvelopeDeserializer protobufEnvelopeDeserializer,
            AsyncClientStreamingCall<ProtobufMessage, ProtobufMessage> callStream,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var all = Task.WhenAll(callStream.ResponseAsync, callStream.ResponseHeadersAsync);
                await Task.WhenAny(all, Task.Delay(Timeout.Infinite, cancellationToken));

                cancellationToken.ThrowIfCancellationRequested();

                return protobufEnvelopeDeserializer.Deserialize(
                    await callStream.ResponseAsync,
                    await callStream.ResponseHeadersAsync);
            }
            catch (RpcException ex)
            {
                return protobufEnvelopeDeserializer.ErrorDeserializer.DeserializeException(ex);
            }
        }

        //TODO Try catch could not be applied here!
        public static async IAsyncEnumerable<Envelope> Deserialize(
            this IProtobufEnvelopeDeserializer protobufEnvelopeDeserializer,
            AsyncDuplexStreamingCall<ProtobufMessage, ProtobufMessage> callStream,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = callStream.ResponseStream.ReadAllAsync(cancellationToken);

            await foreach (var response in responses.WithCancellation(cancellationToken))
            {
                yield return protobufEnvelopeDeserializer.Deserialize(
                    response,
                    await callStream.ResponseHeadersAsync);
            }
        }

        //TODO Try catch could not be applied here!
        public static async IAsyncEnumerable<Envelope> Deserialize(
            this IProtobufEnvelopeDeserializer protobufEnvelopeDeserializer,
            AsyncServerStreamingCall<ProtobufMessage> callStream,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = callStream.ResponseStream.ReadAllAsync(cancellationToken);

            await foreach (var response in responses.WithCancellation(cancellationToken))
            {
                yield return protobufEnvelopeDeserializer.Deserialize(
                    response,
                    await callStream.ResponseHeadersAsync);
            }
        }

        //TODO Try catch could not be applied here!
        public static async IAsyncEnumerable<Envelope> Deserialize(
            this IProtobufEnvelopeDeserializer protobufEnvelopeDeserializer,
            IAsyncStreamReader<ProtobufMessage> stream,
            Metadata? metadata,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = stream.ReadAllAsync(cancellationToken);

            await foreach (var response in responses.WithCancellation(cancellationToken))
            {
                yield return protobufEnvelopeDeserializer.Deserialize(
                    response,
                    metadata);
            }
        }

        public static GrpcChannelOptions ToGrpcChannelOptions(this GrpcClientEndpointOptions options)
        {
            return new GrpcChannelOptions()
            {
                Credentials = options.Credentials,
            };
        }
    }
}
