using Hades.Transport.Abstraction.Server;

namespace Hades.Transport.Abstraction.Configurators
{
    public interface ITransportServerEndpointConfigurator<TEndpoint, TOptions>
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>
    {
    }
}
