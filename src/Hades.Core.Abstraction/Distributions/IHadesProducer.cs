using System.Threading.Tasks;

namespace Hades.Core.Abstraction.Distributions
{
    public interface IHadesProducer
    {
        Task Produce(Envelope envelope);
    }
}
