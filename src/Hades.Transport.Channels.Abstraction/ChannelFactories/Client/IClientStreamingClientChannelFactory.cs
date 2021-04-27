﻿using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IClientStreamingClientChannelFactory :
        IHadesClientChannelFactory<IClientStreamingClientChannel, IClientStreamingInvoker, ICorrelativeSingleEndpointReader, IStreamEndpointWriter>
    {
    }
}