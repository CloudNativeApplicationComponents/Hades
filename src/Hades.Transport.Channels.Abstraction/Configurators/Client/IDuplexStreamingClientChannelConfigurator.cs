using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IDuplexStreamingClientChannelConfigurator : IClientChannelConfigurator,
        IClientChannelInterceptionConfigurator<IDuplexStreamingClientChannelConfigurator, IDuplexStreamingInterceptor>
    {
    }
}
