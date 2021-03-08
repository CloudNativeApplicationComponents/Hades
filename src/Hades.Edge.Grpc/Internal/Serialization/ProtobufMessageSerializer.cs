using Google.Protobuf;
using Grpc.Core;
using Hades.Edge.Grpc.Serialization;
using System.Reflection;

namespace Hades.Edge.Grpc.Internal.Serialization
{
    internal class ProtobufMessageSerializer : IProtobufMessageSerializer, IProtobufMessageDeserializer
    {
        public virtual void SerializeMessage<T>(T message, SerializationContext context)
        where T : IMessage<T>
        {
#if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
            if (message is IBufferMessage)
            {
                context.SetPayloadLength(message.CalculateSize());
                MessageExtensions.WriteTo(message, context.GetBufferWriter());
                context.Complete();
                return;
            }
#endif
            context.Complete(MessageExtensions.ToByteArray(message));
        }

        public virtual T DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser)
            where T : IMessage<T>
        {
#if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
            if (MessageCache<T>.IsBufferMessage)
            {
                return parser.ParseFrom(context.PayloadAsReadOnlySequence());
            }
#endif
            return parser.ParseFrom(context.PayloadAsNewBuffer());
        }

        static class MessageCache<T>
        {
            public static readonly bool IsBufferMessage =
                IntrospectionExtensions.GetTypeInfo(typeof(IBufferMessage)).IsAssignableFrom(typeof(T));
        }
    }
}
