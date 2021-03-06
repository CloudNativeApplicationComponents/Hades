using CloudNativeApplicationComponents.Utils;
using CloudNativeApplicationComponents.Utils.Grpc;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hades.Connector.Grpc.Server;
using Spear.Abstraction;
using System;
using System.Threading.Tasks;

namespace Hades.Connector.Grpc.Internal.Server
{
    internal class ServerServiceDefinitionBuilder : ISpearServerMethodDefinitionVisitor
    {
        private readonly IProtobufMessageSerializer _serializer;
        private readonly IProtobufMessageDeserializer _deserializer;

        public ServerServiceDefinitionBuilder(IProtobufMessageSerializer serializer,
            IProtobufMessageDeserializer deserializer)
        {
            _serializer = serializer
                ?? throw new ArgumentNullException(nameof(serializer));

            _deserializer = deserializer
                ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public ServerServiceDefinition Build(ISpearServiceDefinition service)
        {

            var builder = ServerServiceDefinition.CreateBuilder();

            service.Methods.Foreach(method =>
            {
                var context = new AggregationContext();
                var info = (builder, service);
                context.Add(info);

                method.Accept(this, context);
            });
            return builder.Build();
        }

        protected virtual Method<TRequest, TResponse> CreateMethod<TRequest, TResponse>(string serviceName, string methodName, MethodType methodType)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
        {
            var requestParser = new MessageParser<TRequest>(() => new TRequest()).WithDiscardUnknownFields(false);
            var responseParser = new MessageParser<TResponse>(() => new TResponse()).WithDiscardUnknownFields(false);
            var requestMarshaller = Marshallers.Create(_serializer.SerializeMessage, context => _deserializer.DeserializeMessage(context, requestParser));
            var responseMarshaller = Marshallers.Create(_serializer.SerializeMessage, context => _deserializer.DeserializeMessage(context, responseParser));

            return new Method<TRequest, TResponse>(methodType, serviceName, methodName, requestMarshaller, responseMarshaller);
        }

        void ISpearServerMethodDefinitionVisitor.Visit(ServerEventMethodDefinition method, AggregationContext context)
        {
            var (builder, service) = context.Find<(ServerServiceDefinition.Builder, ISpearServiceDefinition)>();

            var grpcMethod = CreateMethod<ProtobufMessage, Empty>(service.Name, method.Name, MethodType.Unary);
            builder.AddMethod(grpcMethod, ExecuteEvent);
        }

        void ISpearServerMethodDefinitionVisitor.Visit(SpearServerUnaryMethodDefinition method, AggregationContext context)
        {
            var (builder, service) = context.Find<(ServerServiceDefinition.Builder, ISpearServiceDefinition)>();

            var grpcMethod = CreateMethod<ProtobufMessage, ProtobufMessage>(service.Name, method.Name, MethodType.Unary);
            builder.AddMethod(grpcMethod, ExecuteUnary);
        }

        void ISpearServerMethodDefinitionVisitor.Visit(SpearServerClientStreamingMethodDefinition method, AggregationContext context)
        {
            var (builder, service) = context.Find<(ServerServiceDefinition.Builder, ISpearServiceDefinition)>();

            var grpcMethod = CreateMethod<ProtobufMessage, ProtobufMessage>(service.Name, method.Name, MethodType.ClientStreaming);
            builder.AddMethod(grpcMethod, ExecuteClientStreaming);
        }

        void ISpearServerMethodDefinitionVisitor.Visit(SpearServerServerStreamingMethodDefinition method, AggregationContext context)
        {
            var (builder, service) = context.Find<(ServerServiceDefinition.Builder, ISpearServiceDefinition)>();
            var grpcMethod = CreateMethod<ProtobufMessage, ProtobufMessage>(service.Name, method.Name, MethodType.ServerStreaming);

            builder.AddMethod(grpcMethod, ExecuteServerStreaming);
        }

        void ISpearServerMethodDefinitionVisitor.Visit(SpearServerDuplexStreamingMethodDefinition method, AggregationContext context)
        {

            var (builder, service) = context.Find<(ServerServiceDefinition.Builder, ISpearServiceDefinition)>();
            var grpcMethod = CreateMethod<ProtobufMessage, ProtobufMessage>(service.Name, method.Name, MethodType.DuplexStreaming);

            builder.AddMethod(grpcMethod, ExecuteDuplexStreaming);
        }



        private async Task<Empty> ExecuteEvent(ProtobufMessage request, ServerCallContext context)
        {
            var cntx = new AggregationContext();
            cntx.Add(context);

            throw new NotImplementedException();
        }

        private async Task<ProtobufMessage> ExecuteUnary(ProtobufMessage request, ServerCallContext context)
        {
            var cntx = new AggregationContext();
            cntx.Add(context);

            throw new NotImplementedException();
        }

        private async Task<ProtobufMessage> ExecuteClientStreaming(IAsyncStreamReader<ProtobufMessage> requestStream, ServerCallContext context)
        {
            var cntx = new AggregationContext();
            cntx.Add(context);
            var requets = requestStream.ReadAllAsync(context.CancellationToken);

            throw new NotImplementedException();
        }

        private async Task ExecuteServerStreaming(ProtobufMessage request, IServerStreamWriter<ProtobufMessage> responseStream, ServerCallContext context)
        {
            var cntx = new AggregationContext();
            cntx.Add(context);

            throw new NotImplementedException();

            //await foreach (var item in result)
            //{
            //    await responseStream.WriteAsync(item);
            //}
        }

        private async Task ExecuteDuplexStreaming(IAsyncStreamReader<ProtobufMessage> requestStream, IServerStreamWriter<ProtobufMessage> responseStream, ServerCallContext context)
        {
            var cntx = new AggregationContext();
            cntx.Add(context);
            var requets = requestStream.ReadAllAsync(context.CancellationToken);
            throw new NotImplementedException();

            //await foreach (var item in result)
            //{
            //    await responseStream.WriteAsync(item);
            //}
        }
    }
}
