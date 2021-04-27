using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ICorrelativeStreamEndpointWriter : IEndpointWriter
    {
        Task WriteAsync(
            ICorrelativeSubmission correlativeSubmission,
            IAsyncEnumerable<Envelope> envelopeStream,
            CancellationToken cancellationToken = default);
    }
}
