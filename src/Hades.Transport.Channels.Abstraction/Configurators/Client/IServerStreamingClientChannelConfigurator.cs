using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IServerStreamingClientChannelConfigurator : IClientChannelConfigurator,
        IClientChannelInterceptionConfigurator<IServerStreamingClientChannelConfigurator, IServerStreamingInterceptor>
    {
    }
}
