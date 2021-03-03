using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IClientStreamingServerChannelConfigurator : IServerChannelConfigurator,
        IServerChannelInterceptionConfigurator<IClientStreamingServerChannelConfigurator, IClientStreamingInterceptor>
    {
    }
}
