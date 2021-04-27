using Hades.Transport.Abstraction.Client;

namespace Hades.Transport.Abstraction.Configurators
{
    public interface ITransportClientEndpointConfigurator<TEndpoint, TOptions>
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>
    {
    }
}
