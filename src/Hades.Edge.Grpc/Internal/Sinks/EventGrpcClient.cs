using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using System;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class EventGrpcClient : ClientBase<EventGrpcClient>
    {
        private readonly Method<ProtobufMessage, Empty> _method;
        public EventGrpcClient(ChannelBase channel, 
            Method<ProtobufMessage, Empty> method) 
            : base(channel)
        {
            _method = method 
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected EventGrpcClient(ClientBaseConfiguration configuration, 
            Method<ProtobufMessage, Empty> method)
            : base(configuration)
        {
            _method = method 
                ?? throw new ArgumentNullException(nameof(method));
        }

        public virtual AsyncUnaryCall<Empty> Send(ProtobufMessage request, 
            CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall(_method, null, options, request);
        }

        protected override EventGrpcClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new EventGrpcClient(configuration, _method);
        }
    }
}
