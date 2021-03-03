using Grpc.Core;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Internal.Messages;

namespace Hades.Transport.Grpc.Internal.Serialization
{
    internal interface IProtobufEnvelopeSerializer
    {
        IRpcExceptionEnvelopeSerializer ErrorSerializer { get; }
        (ProtobufMessage, Metadata) Serialize(Envelope envelope);
    }
}
