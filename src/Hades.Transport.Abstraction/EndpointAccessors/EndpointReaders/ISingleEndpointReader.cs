using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ISingleEndpointReader : IEndpointReader
    {
        Task<(Envelope, ICorrelativeSubmission)> ReadAsync(
            CancellationToken cancellationToken = default);
    }
}
