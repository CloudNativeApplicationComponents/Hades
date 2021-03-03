using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Client.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class ClientStreamingClientChannelConfigurator :
        ChannelConfiguratorBase<IClientStreamingInterceptor>,
        IClientStreamingClientChannelConfigurator
    {
        public ClientStreamingClientChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IClientStreamingClientChannelFactory, ClientStreamingClientChannelFactory>();
        }

        public IClientStreamingClientChannelConfigurator Interceptor(IClientStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IClientStreamingClientChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IClientStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
