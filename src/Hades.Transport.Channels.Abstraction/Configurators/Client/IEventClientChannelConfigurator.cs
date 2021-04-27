using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IEventClientChannelConfigurator : IClientChannelConfigurator,
        IClientChannelInterceptionConfigurator<IEventClientChannelConfigurator, IEventInterceptor>
    {
    }
}
