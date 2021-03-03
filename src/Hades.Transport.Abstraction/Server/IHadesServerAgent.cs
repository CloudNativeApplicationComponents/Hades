using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Abstraction.Server
{
    public interface IHadesServerAgent
    {
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync();
    }
}
