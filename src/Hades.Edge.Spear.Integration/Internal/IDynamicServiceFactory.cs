using Hades.Edge.Abstraction.Services;

namespace Hades.Edge.Spear.Integration.Internal
{
    internal interface IDynamicServiceFactory
    {
        IDynamicService CreateDynamicService(
            string serviceCatalogName,
            string serviceCatalogDataPlan,
            string serviceName,
            string serviceMethodType);
    }
}