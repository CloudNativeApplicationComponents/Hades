using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Server.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class DuplexStreamingServerChannelConfigurator :
        ChannelConfiguratorBase<IDuplexStreamingInterceptor>,
        IDuplexStreamingServerChannelConfigurator
    {
        public DuplexStreamingServerChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IDuplexStreamingServerChannelFactory, DuplexStreamingServerChannelFactory>();
        }

        public IDuplexStreamingServerChannelConfigurator Interceptor(IDuplexStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IDuplexStreamingServerChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IDuplexStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
