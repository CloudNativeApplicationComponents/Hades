using CloudNativeApplicationComponents.Utils;

namespace Hades.Edge.Abstraction.Services
{
    public interface IDynamicServiceVisitor
    {
        void Visit(EventService definition, AggregationContext context);
        void Visit(UnaryService definition, AggregationContext context);
        void Visit(ClientStreamingService definition, AggregationContext context);
        void Visit(ServerStreamingService definition, AggregationContext context);
        void Visit(DuplexStreamingService definition, AggregationContext context);
    }
}
