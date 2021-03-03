using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.ChannelFactories;
using Hades.Transport.Channels.Abstraction.Invokers;
using Hades.Transport.Channels.Internal.Client.Channels;
using System;

namespace Hades.Transport.Channels.Internal.Client.ChannelFactories
{
    internal class UnaryClientChannelFactory : 
        IHadesClientChannelFactory<IUnaryClientChannel, IUnaryInvoker, ICorrelativeSingleEndpointReader, ISingleEndpointWriter>,
        IUnaryClientChannelFactory
    {
        public IUnaryClientChannel Create(IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter> accessor)
        {
            _ = accessor
                ?? throw new ArgumentNullException(nameof(accessor));

            return new UnaryClientChannel(accessor);
        }
    }
}
