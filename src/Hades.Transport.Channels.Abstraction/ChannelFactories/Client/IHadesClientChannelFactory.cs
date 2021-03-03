using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IHadesClientChannelFactory<out TChannel, in TInvoker, in TEndpointReader, in TEndpointWriter>
        where TChannel : IHadesClientChannel<TInvoker>
        where TInvoker : IHadesInvoker
        where TEndpointReader : IEndpointReader
        where TEndpointWriter : IEndpointWriter
    {
        TChannel Create(IClientEndpointAccessor<TEndpointReader, TEndpointWriter> factory);
    }
}
