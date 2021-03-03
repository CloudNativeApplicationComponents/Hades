using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction.Handlers
{
    public interface IClientStreamingHandler : 
        IHadesChannelHandler,
        IClientStreamingInvoker
    {
    }
}