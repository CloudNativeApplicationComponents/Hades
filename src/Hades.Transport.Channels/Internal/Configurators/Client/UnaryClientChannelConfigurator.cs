using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Client.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class UnaryClientChannelConfigurator :
        ChannelConfiguratorBase<IUnaryInterceptor>,
        IUnaryClientChannelConfigurator
    {
        public UnaryClientChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IUnaryClientChannelFactory, UnaryClientChannelFactory>();
        }

        public IUnaryClientChannelConfigurator Interceptor(IUnaryInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IUnaryClientChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IUnaryInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
