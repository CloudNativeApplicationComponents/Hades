using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Client.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class DuplexStreamingClientChannelConfigurator :
        ChannelConfiguratorBase<IDuplexStreamingInterceptor>,
        IDuplexStreamingClientChannelConfigurator
    {
        public DuplexStreamingClientChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IDuplexStreamingClientChannelFactory, DuplexStreamingClientChannelFactory>();
        }

        public IDuplexStreamingClientChannelConfigurator Interceptor(IDuplexStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IDuplexStreamingClientChannelConfigurator Interceptor<TImplementedInterceptor>() 
            where TImplementedInterceptor : IDuplexStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
