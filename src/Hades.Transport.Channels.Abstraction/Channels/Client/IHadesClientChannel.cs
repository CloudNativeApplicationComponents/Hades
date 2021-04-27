using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IHadesClientChannel<out TInvoker> : IHadesChannel
        where TInvoker : IHadesInvoker
    {
        TInvoker CreateInvoker();
    }
}
