using CloudNativeApplicationComponents.Utils;
using System;

namespace Hades.Edge.Abstraction.Services
{
    public abstract class DynamicServiceBase : IDynamicService
    {
        public string ServiceName { get; }
        public string CatalogName { get; }
        public string DataPlane { get; }
        public abstract DynamicServiceType ServiceType { get; }

        protected DynamicServiceBase(string catalogName, string serviceName, string dataPlane)
        {
            if (string.IsNullOrWhiteSpace(catalogName))
                throw new ArgumentNullException(catalogName);

            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentNullException(serviceName);

            if (string.IsNullOrWhiteSpace(dataPlane))
                throw new ArgumentNullException(dataPlane);

            CatalogName = catalogName;
            ServiceName = serviceName;
            DataPlane = dataPlane;
        }

        public abstract void Accept(IDynamicServiceVisitor visitor, AggregationContext context);
    }
}
