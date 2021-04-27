using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Grpc.Abstraction.Options;

namespace Hades.Transport.Grpc.Abstraction.Server
{
    public interface IGrpcTransportServerEndpoint : 
        IHadesTransportServerEndpoint<GrpcServerEndpointOptions>,
        IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>
    {
        IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter> Event();
        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter> Unary();
        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter> ClientStreaming();
        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter> ServerStreaming();
        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter> DuplexStreaming();
    }
}
