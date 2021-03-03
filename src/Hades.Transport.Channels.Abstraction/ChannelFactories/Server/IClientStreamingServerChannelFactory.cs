using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IClientStreamingServerChannelFactory :
        IHadesServerChannelFactory<IClientStreamingServerChannel, IClientStreamingHandler, IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>
    {
    }
}
