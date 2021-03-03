using System.Threading.Tasks;

namespace Hades.Core.Abstraction.Protocols
{
    public interface IHadesAgent
    {
        Task Start();
        Task Stop();
    }
}
