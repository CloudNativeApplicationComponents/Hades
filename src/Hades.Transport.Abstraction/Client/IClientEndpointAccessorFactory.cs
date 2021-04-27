using Hades.Transport.Abstraction.EndpointAccessors;

namespace Hades.Transport.Abstraction.Client
{
    public interface IClientEndpointAccessorFactory<TEndpointReader, TEndpointWriter> : IHadesTransportClientEndpoint
        where TEndpointReader : IEndpointReader
        where TEndpointWriter : IEndpointWriter
    {
        IClientEndpointAccessor<TEndpointReader, TEndpointWriter> Create();
    }
}
