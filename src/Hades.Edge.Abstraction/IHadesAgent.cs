using System.Threading.Tasks;

namespace Hades.Edge.Abstraction
{
    public interface IHadesAgent
    {
        Task Start();
        Task Stop();
    }
}
