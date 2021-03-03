using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Server.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class UnaryServerChannelConfigurator :
        ChannelConfiguratorBase<IUnaryInterceptor>,
        IUnaryServerChannelConfigurator
    {
        public UnaryServerChannelConfigurator(IServiceCollection collection) 
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IUnaryServerChannelFactory, UnaryServerChannelFactory>();
        }

        public IUnaryServerChannelConfigurator Interceptor(IUnaryInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IUnaryServerChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IUnaryInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
