using System.Collections.Generic;

namespace Hades.Edge.Abstraction.Services
{
    public interface IDynamicServiceDiscovery
    {
        IEnumerable<IDynamicService> GetDynamicServices();
        IEnumerable<IDynamicService> GetDynamicServices(string dataPlane);
    }
}
