using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Invokers;
using Hades.Transport.Channels.Internal.Client.Channels;
using System;

namespace Hades.Transport.Channels.Internal.Client.ChannelFactories
{
    internal class ServerStreamingClientChannelFactory :
        IHadesClientChannelFactory<IServerStreamingClientChannel, IServerStreamingInvoker, ICorrelativeStreamEndpointReader, ISingleEndpointWriter>,
        IServerStreamingClientChannelFactory
    {
        public IServerStreamingClientChannel Create(IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter> accessor)
        {
            _ = accessor
                ?? throw new ArgumentNullException(nameof(accessor));

            return new ServerStreamingClientChannel(accessor);
        }
    }
}
