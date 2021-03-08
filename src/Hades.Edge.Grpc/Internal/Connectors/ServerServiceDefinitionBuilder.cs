using CloudNativeApplicationComponents.Utils;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using CloudNativeApplicationComponents.Utils.Grpc;
using Hades.Edge.Grpc.Serialization;
using System;
using System.Threading.Tasks;
using Hades.Edge.Grpc.Internal.Messages;
using Hades.Edge.Abstraction.Services;
using System.Collections.Generic;

namespace Hades.Edge.Grpc.Internal.Connectors
{
    internal partial class ServerServiceDefinitionBuilder : IDynamicServiceVisitor
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

        public ServerServiceDefinition Build(IEnumerable<IDynamicService> services)
        {

            var builder = ServerServiceDefinition.CreateBuilder();

            services.Foreach(service =>
            {
                var context = new AggregationContext();
                context.Add(builder);

                service.Accept(this, context);
            });
            return builder.Build();
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
