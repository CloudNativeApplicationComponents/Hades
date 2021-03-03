using Hades.Edge.Abstraction.Services;
using System;

namespace Hades.Edge.Spear.Integration.Internal
{
    //TODO ask about refrencing spear.abstraction to use enum string here
    internal class DynamicServiceFactory : IDynamicServiceFactory
    {
        public IDynamicService CreateDynamicService(
            string serviceCatalogName,
            string serviceCatalogDataPlan,
            string serviceName,
            string serviceMethodType)
        {
            return (serviceMethodType.ToLowerInvariant()) switch
            {
                "event" => new EventService(serviceCatalogName, serviceName, serviceCatalogDataPlan),
                "unary" => new UnaryService(serviceCatalogName, serviceName, serviceCatalogDataPlan),
                "clientstreaming" => new ClientStreamingService(serviceCatalogName, serviceName, serviceCatalogDataPlan),
                "serverstreaming" => new ServerStreamingService(serviceCatalogName, serviceName, serviceCatalogDataPlan),
                "duplexstreaming" => new DuplexStreamingService(serviceCatalogName, serviceName, serviceCatalogDataPlan),
                _ => throw new NotImplementedException(),
            };
        }
    }
}