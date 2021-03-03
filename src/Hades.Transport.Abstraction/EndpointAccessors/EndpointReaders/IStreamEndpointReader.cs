using Hades.Transport.Abstraction.Messaging;
using System.Collections.Generic;
using System.Threading;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface IStreamEndpointReader : IEndpointReader
    {
        IAsyncEnumerable<Envelope> ReadAsync(
            CancellationToken cancellationToken = default);
    }
}
