using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface IStreamEndpointObserver : IEndpointObserver
    {
        Task OnReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken = default);
    }
}
