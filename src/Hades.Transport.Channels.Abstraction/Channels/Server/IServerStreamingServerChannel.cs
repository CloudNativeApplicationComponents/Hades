using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IServerStreamingServerChannel :
        IHadesServerChannel<IServerStreamingHandler>
    {
    }
}