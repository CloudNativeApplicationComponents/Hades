using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Grpc.Abstraction.Options;

namespace Hades.Transport.Grpc.Abstraction.Client
{
    public interface IGrpcTransportClientEndpoint : 
        IHadesTransportClientEndpoint<GrpcClientEndpointOptions>,
        IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>,
        IClientEndpointAccessorFactory<NopEndpointReader, ISingleEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
    {
        IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter> Event();
        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter> Unary();
        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter> ClientStreaming();
        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter> ServerStreaming();
        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter> DuplexStreaming();
    }
}
