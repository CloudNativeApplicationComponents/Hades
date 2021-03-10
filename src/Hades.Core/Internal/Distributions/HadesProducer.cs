using Hades.Core.Abstraction.Distributions;
using System;
using System.Threading.Tasks;

namespace Hades.Core.Internal.Distributions
{
    internal class HadesProducer : IHadesProducer
    {
        private readonly IHadesBroker _hadesBroker;
        public HadesProducer(IHadesBroker hadesBroker)
        {
            _hadesBroker = hadesBroker
                ?? throw new ArgumentNullException(nameof(hadesBroker));
        }
        public async Task Produce(Envelope envelope)
        {
           await _hadesBroker.Publish(envelope);
        }
    }
}
