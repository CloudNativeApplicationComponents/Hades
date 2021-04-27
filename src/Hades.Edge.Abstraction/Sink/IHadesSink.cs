using Hades.Edge.Abstraction.Features;
using Hades.Transport.Abstraction;

namespace Hades.Edge.Abstraction.Sink
{
    public interface IHadesSink : IFeaatureContainer
    {
        DataPlane DataPlane { get; }
    }
}
