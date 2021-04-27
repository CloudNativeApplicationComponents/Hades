using Hades.Edge.Abstraction.Connectors;
using Hades.Edge.Abstraction.Features;
using Hades.Edge.Abstraction.Sink;
using Hades.Transport.Abstraction;

namespace Hades.Edge.Abstraction
{
    public interface IHadesHub : IFeaatureContainer
    {
        IHadesConnector Connector(DataPlane dataPlane);
        IHadesSink Sink(DataPlane dataPlane);
    }
}
