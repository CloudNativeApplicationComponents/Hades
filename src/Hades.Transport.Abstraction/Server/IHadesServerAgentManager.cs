using Microsoft.Extensions.Hosting;

namespace Hades.Transport.Abstraction.Server
{
    public interface IHadesServerAgentManager : IHostedService
    {
        void AddAgent(IHadesServerAgent agent);
    }
}
