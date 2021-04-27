using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IClientChannelInterceptionConfigurator<TConfigurator, in TInterceptor>
        where TConfigurator : IClientChannelConfigurator
        where TInterceptor : IChannelInterceptor
    {
        TConfigurator Interceptor(TInterceptor interceptor);
        TConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : TInterceptor;
    }
}
