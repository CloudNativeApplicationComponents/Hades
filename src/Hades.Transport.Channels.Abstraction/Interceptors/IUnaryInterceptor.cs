using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Invokers;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Interceptors
{
    public interface IUnaryInterceptor :
        IChannelInterceptor
    {
        Task<Envelope> CallAsync(
            IUnaryInvoker invoker,
            Envelope request,
            CancellationToken cancellationToken = default);
    }
}