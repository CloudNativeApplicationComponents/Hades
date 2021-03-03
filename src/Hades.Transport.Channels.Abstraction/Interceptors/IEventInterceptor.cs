using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Channels.Abstraction.Invokers;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Channels.Abstraction.Interceptors
{
    public interface IEventInterceptor :
        IChannelInterceptor
    {
        Task PublishAsync(
            IEventInvoker invoker, 
            Envelope envelope, 
            CancellationToken cancellationToken = default);
    }
}