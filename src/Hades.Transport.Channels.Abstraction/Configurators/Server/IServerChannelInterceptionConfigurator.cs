using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IServerChannelInterceptionConfigurator<TConfigurator, in TInterceptor>
        where TConfigurator : IServerChannelConfigurator
        where TInterceptor : IChannelInterceptor
    {
        TConfigurator Interceptor(TInterceptor interceptor);
        TConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : TInterceptor;
    }
}
