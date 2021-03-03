using Grpc.Core;
using Hades.Transport.Abstraction.Messaging;

namespace Hades.Transport.Grpc.Internal.Serialization
{
    internal interface IRpcExceptionEnvelopeDeserializer
    {
        Envelope DeserializeException(RpcException rpcException, Metadata? metadata = default);
    }
}
