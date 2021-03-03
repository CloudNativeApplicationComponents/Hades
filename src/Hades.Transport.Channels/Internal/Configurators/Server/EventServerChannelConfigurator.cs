using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Server.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class EventServerChannelConfigurator :
        ChannelConfiguratorBase<IEventInterceptor>,
        IEventServerChannelConfigurator
    {
        public EventServerChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IEventServerChannelFactory, EventServerChannelFactory>();
        }

        public IEventServerChannelConfigurator Interceptor(IEventInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IEventServerChannelConfigurator Interceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : IEventInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
