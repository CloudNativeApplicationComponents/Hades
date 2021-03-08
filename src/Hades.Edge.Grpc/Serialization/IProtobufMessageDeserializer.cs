using Google.Protobuf;
using Grpc.Core;

namespace Hades.Edge.Grpc.Serialization
{
    public interface IProtobufMessageDeserializer
    {
        T DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser)
            where T : IMessage<T>;
    }
}
