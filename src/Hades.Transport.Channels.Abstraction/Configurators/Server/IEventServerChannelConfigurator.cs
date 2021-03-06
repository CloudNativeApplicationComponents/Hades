﻿using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IEventServerChannelConfigurator : IServerChannelConfigurator,
        IServerChannelInterceptionConfigurator<IEventServerChannelConfigurator, IEventInterceptor>
    {
    }
}
