using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ISingleEndpointObserver : IEndpointObserver
    {
        Task OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            Envelope envelope,
            CancellationToken cancellationToken = default);
    }
}
