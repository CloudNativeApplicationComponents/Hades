using Hades.Broker.Abstraction.Configurations;
using Hades.Broker.Transport.Integration.Abstraction.Configurators;
using Hades.Broker.Transport.Integration.Internal.Configurators;
using Hades.Transport.Channels.Abstraction.Configurators;
using System;

namespace Hades.Broker.Transport.Integration
{
    public static class ServerChannelExtension
    {
        public static IUnaryServerChannelConfigurator AddConsumer(
            this IUnaryServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IUnaryServerChannelConfigurator AddProducer(
            this IUnaryServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }


        public static IClientStreamingServerChannelConfigurator AddConsumer(
            this IClientStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IClientStreamingServerChannelConfigurator AddProducer(
            this IClientStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }



        public static IServerStreamingServerChannelConfigurator AddConsumer(
            this IServerStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IServerStreamingServerChannelConfigurator AddProducer(
            this IServerStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }


        public static IDuplexStreamingServerChannelConfigurator AddConsumer(
            this IDuplexStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerConsumerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerConsumerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
        public static IDuplexStreamingServerChannelConfigurator AddProducer(
            this IDuplexStreamingServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }



        public static IEventServerChannelConfigurator AddProducer(
            this IEventServerChannelConfigurator channelConfigurator,
            IBrokerConfigurationBuilder builder,
            Action<IBrokerProducerOptionsConfigurator> configure)
        {
            _ = builder ??
                throw new ArgumentNullException(nameof(builder));

            var configurator = new BrokerProducerConfigurator();
            configure?.Invoke(configurator);

            return channelConfigurator;
        }
    }
}
