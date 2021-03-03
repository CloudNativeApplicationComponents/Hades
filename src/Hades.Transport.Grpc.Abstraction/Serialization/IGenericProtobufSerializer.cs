using Google.Protobuf;
using Grpc.Core;

namespace Hades.Transport.Grpc.Abstraction.Serialization
{
    public interface IGenericProtobufSerializer
    {
        void SerializeMessage<T>(T message, SerializationContext context)
        where T : IMessage<T>;
    }
}
