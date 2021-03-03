using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IDuplexStreamingServerChannelConfigurator : IServerChannelConfigurator,
        IServerChannelInterceptionConfigurator<IDuplexStreamingServerChannelConfigurator, IDuplexStreamingInterceptor>
    {
    }
}
