using Grpc.Core;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Internal.Messages;

namespace Hades.Transport.Grpc.Internal.Serialization
{
    internal interface IProtobufEnvelopeDeserializer
    {
        IRpcExceptionEnvelopeDeserializer ErrorDeserializer { get; }
        Envelope Deserialize(ProtobufMessage protobufMessage, Metadata? metadata = default);
    }
}
