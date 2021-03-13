using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hades.Edge.Abstraction.Services
{
    public interface IDynamicServiceDiscovery
    {
        Task<IEnumerable<IDynamicService>> GetDynamicServices();
        Task<IEnumerable<IDynamicService>> GetDynamicServices(string dataPlane);
    }
}
