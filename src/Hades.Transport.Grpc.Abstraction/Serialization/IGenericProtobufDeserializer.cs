using Google.Protobuf;
using Grpc.Core;

namespace Hades.Transport.Grpc.Abstraction.Serialization
{
    public interface IGenericProtobufDeserializer
    {
        T DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser)
            where T : IMessage<T>;
    }
}
