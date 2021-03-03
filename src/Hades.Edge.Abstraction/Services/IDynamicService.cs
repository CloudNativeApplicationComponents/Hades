using CloudNativeApplicationComponents.Utils;

namespace Hades.Edge.Abstraction.Services
{
    public interface IDynamicService
    {
        string ServiceName { get; }
        string CatalogName { get; }
        DynamicServiceType ServiceType { get; }
        string DataPlane { get; }
        void Accept(IDynamicServiceVisitor visitor, AggregationContext context);
    }
}
