using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Client.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class ServerStreamingClientChannelConfigurator :
        ChannelConfiguratorBase<IServerStreamingInterceptor>,
        IServerStreamingClientChannelConfigurator
    {
        public ServerStreamingClientChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IServerStreamingClientChannelFactory, ServerStreamingClientChannelFactory>();
        }

        public IServerStreamingClientChannelConfigurator Interceptor(IServerStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IServerStreamingClientChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IServerStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
