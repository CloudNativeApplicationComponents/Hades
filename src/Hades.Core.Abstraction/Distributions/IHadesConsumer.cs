using System;
using System.Threading.Tasks;

namespace Hades.Core.Abstraction.Distributions
{
    public interface IHadesConsumer
    {
        Task Consume(Envelope envelope);
    }
}
