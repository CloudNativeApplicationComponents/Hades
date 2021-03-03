using Hades.Broker.Abstraction.Configurations;
using Hades.Broker.Transport.Integration.Abstraction.Configurators;
using Hades.Broker.Transport.Integration.Internal.Configurators;
using Hades.Transport.Channels.Abstraction.Configurators;
using System;

namespace Hades.Broker.Transport.Integration
{
    public static class ClientChannelExtension
    {
        public static IUnaryClientChannelConfigurator AddConsumer(
            this IUnaryClientChannelConfigurator channelConfigurator, 
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IUnaryClientChannelConfigurator AddProducer(
            this IUnaryClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }


        public static IClientStreamingClientChannelConfigurator AddConsumer(
            this IClientStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IClientStreamingClientChannelConfigurator AddProducer(
            this IClientStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }



        public static IServerStreamingClientChannelConfigurator AddConsumer(
            this IServerStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IServerStreamingClientChannelConfigurator AddProducer(
            this IServerStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }


        public static IDuplexStreamingClientChannelConfigurator AddConsumer(
            this IDuplexStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IDuplexStreamingClientChannelConfigurator AddProducer(
            this IDuplexStreamingClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }



        public static IEventClientChannelConfigurator AddConsumer(
            this IEventClientChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = channelConfigurator ??
                throw new ArgumentNullException(nameof(channelConfigurator));
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
    }
}
