using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Hades.Transport.Channels.Internal.Client.ChannelFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class EventClientChannelConfigurator :
        ChannelConfiguratorBase<IEventInterceptor>,
        IEventClientChannelConfigurator
    {
        public EventClientChannelConfigurator(IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IEventClientChannelFactory, EventClientChannelFactory>();
        }

        public IEventClientChannelConfigurator Interceptor(IEventInterceptor interceptor)
        {
            AddInterceptor(interceptor);
            return this;
        }

        public IEventClientChannelConfigurator Interceptor<TImplementedInterceptor>() 
            where TImplementedInterceptor : IEventInterceptor
        {
            AddInterceptor<TImplementedInterceptor>();
            return this;
        }
    }
}
