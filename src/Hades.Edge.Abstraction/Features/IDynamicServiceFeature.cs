using Hades.Edge.Abstraction.Services;
using System.Collections.Generic;

namespace Hades.Edge.Abstraction.Features
{
    public interface IDynamicServiceFeature : IHadesFeature
    {
        IEnumerable<IDynamicService> GetDynamicServices();
    }
}
