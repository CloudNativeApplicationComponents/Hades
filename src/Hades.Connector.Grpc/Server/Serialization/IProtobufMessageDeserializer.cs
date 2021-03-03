using Google.Protobuf;
using Grpc.Core;

namespace Hades.Connector.Grpc.Server
{
    public interface IProtobufMessageDeserializer
    {
        T DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser)
            where T : IMessage<T>;
    }
}
