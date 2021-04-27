﻿using Hades.Transport.Channels.Abstraction.Interceptors;

namespace Hades.Transport.Channels.Abstraction.Configurators
{
    public interface IServerStreamingServerChannelConfigurator : IServerChannelConfigurator,
        IServerChannelInterceptionConfigurator<IServerStreamingServerChannelConfigurator, IServerStreamingInterceptor>
    {
    }
}
