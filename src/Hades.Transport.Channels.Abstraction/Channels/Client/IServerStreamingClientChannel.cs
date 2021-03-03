using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IServerStreamingClientChannel :
        IHadesClientChannel<IServerStreamingInvoker>
    {
    }
}