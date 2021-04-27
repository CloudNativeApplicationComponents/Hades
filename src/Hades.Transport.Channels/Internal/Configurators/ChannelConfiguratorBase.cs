using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Channels.Abstraction.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Hades.Transport.Channels.Internal.Configurators
{
    internal class ChannelConfiguratorBase<TInterceptor> :
        ServiceCollectionConfigurator
        where TInterceptor : IChannelInterceptor
    {
        protected List<Func<IServiceProvider, TInterceptor>> InterceptorFactories { get; }

        public ChannelConfiguratorBase(IServiceCollection collection)
            : base(collection)
        {
            InterceptorFactories = new List<Func<IServiceProvider, TInterceptor>>();
        }

        protected void AddInterceptor(TInterceptor interceptor)
        {
            _ = interceptor ??
                throw new ArgumentNullException(nameof(interceptor));

            InterceptorFactories.Add(sp => interceptor);
        }

        protected void AddInterceptor<TImplementedInterceptor>()
            where TImplementedInterceptor : TInterceptor
        {
            InterceptorFactories.Add(sp => sp.GetRequiredService<TImplementedInterceptor>());
        }
    }
}
