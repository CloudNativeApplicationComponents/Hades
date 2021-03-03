using Google.Protobuf;
using Grpc.Core;

namespace Hades.Connector.Grpc.Server
{
    public interface IProtobufMessageSerializer
    {
        void SerializeMessage<T>(T message, SerializationContext context)
        where T : IMessage<T>;
    }
}
