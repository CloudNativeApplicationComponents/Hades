using Grpc.Core;
using Hades.Transport.Abstraction.Messaging;

namespace Hades.Transport.Grpc.Internal.Serialization
{
    internal interface IRpcExceptionEnvelopeSerializer
    {
        (RpcException, Metadata) SerializeException(Envelope envelope);
    }
}
