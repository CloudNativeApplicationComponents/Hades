using Hades.Transport.Abstraction.Server;
using Hades.Transport.Abstraction.EndpointAccessors;
using System;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Internal.Server.Channels;
using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Internal.Server.ChannelFactories
{
    internal class ClientStreamingServerChannelFactory :
        IHadesServerChannelFactory<IClientStreamingServerChannel, IClientStreamingHandler, IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IClientStreamingServerChannelFactory
    {
        public IClientStreamingServerChannel Create(
            IClientStreamingHandler handler,
            IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter> accessor)
        {
            _ = handler
                ?? throw new ArgumentNullException(nameof(handler));
            _ = accessor
                ?? throw new ArgumentNullException(nameof(accessor));

            return new ClientStreamingServerChannel(handler, accessor);
        }
    }
}
