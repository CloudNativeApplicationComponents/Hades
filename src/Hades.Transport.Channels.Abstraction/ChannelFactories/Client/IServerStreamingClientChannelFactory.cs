using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IServerStreamingClientChannelFactory :
        IHadesClientChannelFactory<IServerStreamingClientChannel, IServerStreamingInvoker, ICorrelativeStreamEndpointReader, ISingleEndpointWriter>
    {
    }
}