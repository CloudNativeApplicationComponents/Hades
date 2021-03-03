using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IUnaryClientChannelConfigurator : IClientChannelConfigurator,
        IClientChannelInterceptionConfigurator<IUnaryClientChannelConfigurator, IUnaryInterceptor>
    {
    }
}
