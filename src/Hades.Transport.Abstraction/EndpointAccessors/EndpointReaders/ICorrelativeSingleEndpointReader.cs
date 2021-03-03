using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ICorrelativeSingleEndpointReader : IEndpointReader
    {
        Task<Envelope> ReadAsync(
            ICorrelativeSubmission correlativeSubmission,
            CancellationToken cancellationToken = default);
    }
}
