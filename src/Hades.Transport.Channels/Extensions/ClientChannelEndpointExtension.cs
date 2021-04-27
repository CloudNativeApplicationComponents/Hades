using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using System;

namespace Hades.Transport.Channels
{
    public static class ClientChannelEndpointExtension
    {
        public static IUnaryClientChannel CreateUnaryChannel<TClientEndpoint, TChannelFactory>(
            this TClientEndpoint clientEndpoint, 
            TChannelFactory channelFactory)
            where TClientEndpoint : IHadesTransportClientEndpoint, 
                                    IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
            where TChannelFactory : IUnaryClientChannelFactory
        {
            _ = clientEndpoint
                ?? throw new ArgumentNullException(nameof(clientEndpoint));

            var accessor = clientEndpoint.Create();
            return channelFactory.Create(accessor);
        }

        public static IEventClientChannel CreateEventChannel<TClientEndpoint, TChannelFactory>(
            this TClientEndpoint clientEndpoint, 
            TChannelFactory channelFactory)
            where TClientEndpoint : IHadesTransportClientEndpoint, 
                                    IClientEndpointAccessorFactory<NopEndpointReader, ISingleEndpointWriter>
            where TChannelFactory : IEventClientChannelFactory
        {
            _ = clientEndpoint
                ?? throw new ArgumentNullException(nameof(clientEndpoint));

            var accessor = clientEndpoint.Create();
            return channelFactory.Create(accessor);
        }

        public static IClientStreamingClientChannel CreateClientStreamingChannel<TClientEndpoint, TChannelFactory>(
            this TClientEndpoint clientEndpoint, 
            TChannelFactory channelFactory)
            where TClientEndpoint : IHadesTransportClientEndpoint, 
                                    IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>
            where TChannelFactory : IClientStreamingClientChannelFactory
        {
            _ = clientEndpoint
                ?? throw new ArgumentNullException(nameof(clientEndpoint));

            var accessor = clientEndpoint.Create();
            return channelFactory.Create(accessor);
        }

        public static IServerStreamingClientChannel CreateServerStreamingChannel<TClientEndpoint, TChannelFactory>(
            this TClientEndpoint clientEndpoint, 
            TChannelFactory channelFactory)
            where TClientEndpoint : IHadesTransportClientEndpoint, 
                                    IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>
            where TChannelFactory : IServerStreamingClientChannelFactory
        {
            _ = clientEndpoint
                ?? throw new ArgumentNullException(nameof(clientEndpoint));

            var accessor = clientEndpoint.Create();
            return channelFactory.Create(accessor);
        }

        public static IDuplexStreamingClientChannel CreateDuplexStreamingChannel<TClientEndpoint, TChannelFactory>(
            this TClientEndpoint clientEndpoint, 
            TChannelFactory channelFactory)
            where TClientEndpoint : IHadesTransportClientEndpoint, 
                                    IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>
            where TChannelFactory : IDuplexStreamingClientChannelFactory
        {
            _ = clientEndpoint
                ?? throw new ArgumentNullException(nameof(clientEndpoint));

            var accessor = clientEndpoint.Create();
            return channelFactory.Create(accessor);
        }
    }
}
