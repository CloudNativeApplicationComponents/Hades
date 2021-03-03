using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Invokers;
using Hades.Transport.Channels.Internal.Client.Channels;
using System;

namespace Hades.Transport.Channels.Internal.Client.ChannelFactories
{
    internal class DuplexStreamingClientChannelFactory :
        IHadesClientChannelFactory<IDuplexStreamingClientChannel, IDuplexStreamingInvoker, ICorrelativeStreamEndpointReader, IStreamEndpointWriter>,
        IDuplexStreamingClientChannelFactory
    {
        public IDuplexStreamingClientChannel Create(IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter> accessor)
        {
            _ = accessor
                ?? throw new ArgumentNullException(nameof(accessor));

            return new DuplexStreamingClientChannel(accessor);
        }
    }
}
