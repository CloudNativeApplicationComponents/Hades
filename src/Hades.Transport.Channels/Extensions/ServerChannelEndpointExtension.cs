using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Internal;
using Hades.Transport.Internal.Channels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels
{
    public static class ServerChannelEndpointExtension
    {
        public static IUnaryServerChannel CreateUnaryChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            IUnaryHandler handler)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>
            where TChannelFactory : IUnaryServerChannelFactory
        {
            _ = serverEndpoint
                ?? throw new ArgumentNullException(nameof(serverEndpoint));
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));


            var accessor = serverEndpoint.Create();
            return channelFactory.Create(handler, accessor);
        }

        public static IUnaryServerChannel CreateUnaryChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            Func<Envelope, CancellationToken, Task<Envelope>> invoker)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>
            where TChannelFactory : IUnaryServerChannelFactory
        {
            _ = invoker
                ?? throw new ArgumentNullException(nameof(invoker));

            var handler = new UnaryHandler(invoker);
            return CreateUnaryChannel(serverEndpoint, channelFactory, handler);
        }

        public static IEventServerChannel CreateEventChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            IEventHandler handler)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>
            where TChannelFactory : IEventServerChannelFactory
        {
            _ = serverEndpoint
                ?? throw new ArgumentNullException(nameof(serverEndpoint));
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));

            var accessor = serverEndpoint.Create();
            return channelFactory.Create(handler, accessor);
        }

        public static IEventServerChannel CreateEventChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            Func<Envelope, CancellationToken, Task> invoker)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>
            where TChannelFactory : IEventServerChannelFactory
        {
            _ = invoker
                ?? throw new ArgumentNullException(nameof(invoker));

            var handler = new Internal.EventHandler(invoker);
            return CreateEventChannel(serverEndpoint, channelFactory, handler);
        }

        public static IClientStreamingServerChannel CreateClientStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            IClientStreamingHandler handler)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>
            where TChannelFactory : IClientStreamingServerChannelFactory
        {
            _ = serverEndpoint
                ?? throw new ArgumentNullException(nameof(serverEndpoint));
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));

            var accessor = serverEndpoint.Create();
            return channelFactory.Create(handler, accessor);
        }

        public static IClientStreamingServerChannel CreateClientStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            Func<IAsyncEnumerable<Envelope>, CancellationToken, Task<Envelope>> invoker)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>
            where TChannelFactory : IClientStreamingServerChannelFactory
        {
            _ = invoker
                ?? throw new ArgumentNullException(nameof(invoker));

            var handler = new ClientStreamingHandler(invoker);
            return CreateClientStreamingChannel(serverEndpoint, channelFactory, handler);
        }

        public static IServerStreamingServerChannel CreateServerStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            IServerStreamingHandler handler)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>
            where TChannelFactory : IServerStreamingServerChannelFactory
        {
            _ = serverEndpoint
                ?? throw new ArgumentNullException(nameof(serverEndpoint));
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));

            var accessor = serverEndpoint.Create();
            return channelFactory.Create(handler, accessor);
        }

        public static IServerStreamingServerChannel CreateServerStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            Func<Envelope, CancellationToken, IAsyncEnumerable<Envelope>> invoker)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>
            where TChannelFactory : IServerStreamingServerChannelFactory
        {
            _ = invoker
                ?? throw new ArgumentNullException(nameof(invoker));

            var handler = new ServerStreamingHandler(invoker);
            return CreateServerStreamingChannel(serverEndpoint, channelFactory, handler);
        }

        public static IDuplexStreamingServerChannel CreateDuplexStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            IDuplexStreamingHandler handler)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>
            where TChannelFactory : IDuplexStreamingServerChannelFactory
        {
            _ = serverEndpoint
                ?? throw new ArgumentNullException(nameof(serverEndpoint));
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));

            var accessor = serverEndpoint.Create();
            return channelFactory.Create(handler, accessor);
        }

        public static IDuplexStreamingServerChannel CreateDuplexStreamingChannel<TServerEndpoint, TChannelFactory>(
            this TServerEndpoint serverEndpoint,
            TChannelFactory channelFactory,
            Func<IAsyncEnumerable<Envelope>, CancellationToken, IAsyncEnumerable<Envelope>> invoker)
            where TServerEndpoint : IHadesTransportServerEndpoint, IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>
            where TChannelFactory : IDuplexStreamingServerChannelFactory
        {
            _ = invoker
                ?? throw new ArgumentNullException(nameof(invoker));

            var handler = new DuplexStreamingHandler(invoker);
            return CreateDuplexStreamingChannel(serverEndpoint, channelFactory, handler);
        }
    }
}
