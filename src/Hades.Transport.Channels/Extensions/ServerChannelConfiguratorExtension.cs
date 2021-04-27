using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Internal;
using Hades.Transport.Channels.Internal.Configurators;
using System;

namespace Hades.Transport.Channels
{
    public static class ServerChannelConfiguratorExtension
    {
        public static ITransportServerEndpointConfigurator<TEndpoint, TOptions> AddUnaryChannel<TEndpoint, TOptions>(
            this ITransportServerEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IUnaryServerChannelConfigurator> configure)
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>,
            IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new UnaryServerChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportServerEndpointConfigurator<TEndpoint, TOptions> AddClientStreamingChannel<TEndpoint, TOptions>(
            this ITransportServerEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IClientStreamingServerChannelConfigurator> configure)
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>,
            IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new ClientStreamingServerChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportServerEndpointConfigurator<TEndpoint, TOptions> AddServerStreamingChannel<TEndpoint, TOptions>(
            this ITransportServerEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IServerStreamingServerChannelConfigurator> configure)
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>,
            IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new ServerStreamingServerChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportServerEndpointConfigurator<TEndpoint, TOptions> AddDuplexStreamingChannel<TEndpoint, TOptions>(
            this ITransportServerEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IDuplexStreamingServerChannelConfigurator> configure)
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>,
            IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new DuplexStreamingServerChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportServerEndpointConfigurator<TEndpoint, TOptions> AddEventChannel<TEndpoint, TOptions>(
            this ITransportServerEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IEventServerChannelConfigurator> configure)
        where TEndpoint : IHadesTransportServerEndpoint<TOptions>,
            IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new EventServerChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }
    }
}
