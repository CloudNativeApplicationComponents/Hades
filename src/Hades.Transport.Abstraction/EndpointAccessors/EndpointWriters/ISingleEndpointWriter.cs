using Hades.Transport.Abstraction.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ISingleEndpointWriter : IEndpointWriter
    {
        Task<ICorrelativeSubmission> WriteAsync(
            Envelope envelope,
            CancellationToken cancellationToken = default);
    }
}
