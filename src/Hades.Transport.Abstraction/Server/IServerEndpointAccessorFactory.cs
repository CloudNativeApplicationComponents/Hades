using Hades.Transport.Abstraction.EndpointAccessors;

namespace Hades.Transport.Abstraction.Server
{
    public interface IServerEndpointAccessorFactory<TEndpointObservable, TEndpointWriter>: IHadesTransportServerEndpoint
        where TEndpointObservable : IEndpointObservable
        where TEndpointWriter : IEndpointWriter

    {
        IServerEndpointAccessor<TEndpointObservable, TEndpointWriter> Create();
    }
}
