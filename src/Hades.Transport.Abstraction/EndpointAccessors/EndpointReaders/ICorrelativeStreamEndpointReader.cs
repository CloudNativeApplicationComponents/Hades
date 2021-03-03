using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ICorrelativeStreamEndpointReader : IEndpointReader
    {
        IAsyncEnumerable<Envelope> ReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            CancellationToken cancellationToken = default);
    }
}
