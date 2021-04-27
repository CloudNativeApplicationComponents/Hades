using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IClientStreamingClientChannelConfigurator : IClientChannelConfigurator,
        IClientChannelInterceptionConfigurator<IClientStreamingClientChannelConfigurator, IClientStreamingInterceptor>
    {
    }
}
