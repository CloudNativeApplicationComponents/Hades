using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Server.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class ClientStreamingServerChannelConfigurator :
        ChannelConfiguratorBase<IClientStreamingInterceptor>,
        IClientStreamingServerChannelConfigurator
    {
        public ClientStreamingServerChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IClientStreamingServerChannelFactory, ClientStreamingServerChannelFactory>();
        }

        public IClientStreamingServerChannelConfigurator Interceptor(IClientStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IClientStreamingServerChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IClientStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
