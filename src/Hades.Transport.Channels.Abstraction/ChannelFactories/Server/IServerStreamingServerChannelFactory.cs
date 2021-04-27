using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IServerStreamingServerChannelFactory :
        IHadesServerChannelFactory<IServerStreamingServerChannel, IServerStreamingHandler, ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>
    {
    }
}
