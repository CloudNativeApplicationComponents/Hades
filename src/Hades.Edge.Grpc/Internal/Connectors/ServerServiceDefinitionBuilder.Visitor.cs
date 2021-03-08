using CloudNativeApplicationComponents.Utils;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using Hades.Edge.Abstraction.Services;

namespace Hades.Edge.Grpc.Internal.Connectors
{
    internal partial class ServerServiceDefinitionBuilder : IDynamicServiceVisitor
    {
        void IDynamicServiceVisitor.Visit(EventService service, AggregationContext context)
        {
            var builder = context.Find<ServerServiceDefinition.Builder>();

            var grpcMethod = Utility.CreateMethod<ProtobufMessage, Empty>(service.CatalogName, service.ServiceName, MethodType.Unary, _serializer, _deserializer);
            builder.AddMethod(grpcMethod, ExecuteEvent);
        }

        void IDynamicServiceVisitor.Visit(UnaryService service, AggregationContext context)
        {
            var builder = context.Find<ServerServiceDefinition.Builder>();

            var grpcMethod = Utility.CreateMethod<ProtobufMessage, ProtobufMessage>(service.CatalogName, service.ServiceName, MethodType.Unary, _serializer, _deserializer);
            builder.AddMethod(grpcMethod, ExecuteUnary);
        }

        void IDynamicServiceVisitor.Visit(ClientStreamingService service, AggregationContext context)
        {
            var builder = context.Find<ServerServiceDefinition.Builder>();

            var grpcMethod = Utility.CreateMethod<ProtobufMessage, ProtobufMessage>(service.CatalogName, service.ServiceName, MethodType.ClientStreaming, _serializer, _deserializer);
            builder.AddMethod(grpcMethod, ExecuteClientStreaming);
        }

        void IDynamicServiceVisitor.Visit(ServerStreamingService service, AggregationContext context)
        {
            var builder = context.Find<ServerServiceDefinition.Builder>();

            var grpcMethod = Utility.CreateMethod<ProtobufMessage, ProtobufMessage>(service.CatalogName, service.ServiceName, MethodType.ServerStreaming, _serializer, _deserializer);

            builder.AddMethod(grpcMethod, ExecuteServerStreaming);
        }

        void IDynamicServiceVisitor.Visit(DuplexStreamingService service, AggregationContext context)
        {

            var builder = context.Find<ServerServiceDefinition.Builder>();
            var grpcMethod = Utility.CreateMethod<ProtobufMessage, ProtobufMessage>(service.CatalogName, service.ServiceName, MethodType.DuplexStreaming, _serializer, _deserializer);

            builder.AddMethod(grpcMethod, ExecuteDuplexStreaming);
        }
    }
}
