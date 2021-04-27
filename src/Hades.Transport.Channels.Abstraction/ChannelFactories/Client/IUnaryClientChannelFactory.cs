using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IUnaryClientChannelFactory : 
        IHadesClientChannelFactory<IUnaryClientChannel, IUnaryInvoker, ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
    {
    }
}