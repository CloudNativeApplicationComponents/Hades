using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Server.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class ServerStreamingServerChannelConfigurator :
        ChannelConfiguratorBase<IServerStreamingInterceptor>,
        IServerStreamingServerChannelConfigurator
    {
        public ServerStreamingServerChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IServerStreamingServerChannelFactory, ServerStreamingServerChannelFactory>();
        }

        public IServerStreamingServerChannelConfigurator Interceptor(IServerStreamingInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IServerStreamingServerChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IServerStreamingInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
