using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IUnaryServerChannelConfigurator : IServerChannelConfigurator,
        IServerChannelInterceptionConfigurator<IUnaryServerChannelConfigurator, IUnaryInterceptor>
    {
    }
}
