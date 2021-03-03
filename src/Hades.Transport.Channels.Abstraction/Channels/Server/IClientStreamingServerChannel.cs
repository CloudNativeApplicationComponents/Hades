using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IClientStreamingServerChannel : 
        IHadesServerChannel<IClientStreamingHandler>
    {
    }
}