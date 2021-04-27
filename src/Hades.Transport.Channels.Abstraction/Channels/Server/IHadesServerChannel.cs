using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IHadesServerChannel<in THandler> : IHadesChannel
        where THandler : IHadesChannelHandler
    {
    }
}
