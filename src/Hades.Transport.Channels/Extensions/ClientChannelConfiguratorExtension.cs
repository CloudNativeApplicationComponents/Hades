using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Configurators;
using Hades.Transport.Channels.Internal;
using Hades.Transport.Channels.Internal.Configurators;
using System;

namespace Hades.Transport.Channels
{
    public static class ClientChannelConfiguratorExtension
    {
        public static ITransportClientEndpointConfigurator<TEndpoint, TOptions> AddEventChannel<TEndpoint, TOptions>(
            this ITransportClientEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IEventClientChannelConfigurator> configure)
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>,
            IClientEndpointAccessorFactory<NopEndpointReader, ISingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));
            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new EventClientChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportClientEndpointConfigurator<TEndpoint, TOptions> AddUnaryChannel<TEndpoint, TOptions>(
            this ITransportClientEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IUnaryClientChannelConfigurator> configure)
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>,
            IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new UnaryClientChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportClientEndpointConfigurator<TEndpoint, TOptions> AddClientStreamingChannel<TEndpoint, TOptions>(
            this ITransportClientEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IClientStreamingClientChannelConfigurator> configure)
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>,
            IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new ClientStreamingClientChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportClientEndpointConfigurator<TEndpoint, TOptions> AddServerStreamingChannel<TEndpoint, TOptions>(
            this ITransportClientEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IServerStreamingClientChannelConfigurator> configure)
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>,
            IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new ServerStreamingClientChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

        public static ITransportClientEndpointConfigurator<TEndpoint, TOptions> AddDuplexStreamingChannel<TEndpoint, TOptions>(
            this ITransportClientEndpointConfigurator<TEndpoint, TOptions> endpointConfigurator,
            Action<IDuplexStreamingClientChannelConfigurator> configure)
        where TEndpoint : IHadesTransportClientEndpoint<TOptions>,
            IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>
        {
            _ = endpointConfigurator
                ?? throw new ArgumentNullException(nameof(endpointConfigurator));
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var serviceCollection = ChannelsUtilities.GetServiceCollection(endpointConfigurator, nameof(endpointConfigurator));
            var configurator = new DuplexStreamingClientChannelConfigurator(serviceCollection);
            configure.Invoke(configurator);
            return endpointConfigurator;
        }

    }
}
