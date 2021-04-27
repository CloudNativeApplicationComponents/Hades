using Hades.Edge.Abstraction.Features;
using Hades.Transport.Abstraction;

namespace Hades.Edge.Abstraction.Connectors
{
    public interface IHadesConnector : IFeaatureContainer
    {
        DataPlane DataPlane { get; }
        void Start(ExecutionContext context);
        void Stop();
    }
}
