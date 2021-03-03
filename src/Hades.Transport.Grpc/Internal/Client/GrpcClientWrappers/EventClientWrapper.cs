using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using System;

namespace Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers
{
    internal class EventClientWrapper : ClientBase<EventClientWrapper>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public EventClientWrapper(ChannelBase channel, 
            Method<ProtobufMessage, ProtobufMessage> method) 
            : base(channel)
        {
            _method = method 
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected EventClientWrapper(ClientBaseConfiguration configuration, 
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(configuration)
        {
            _method = method 
                ?? throw new ArgumentNullException(nameof(method));
        }

        public virtual AsyncUnaryCall<ProtobufMessage> Send(ProtobufMessage request, 
            CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall(_method, null, options, request);
        }

        protected override EventClientWrapper NewInstance(ClientBaseConfiguration configuration)
        {
            return new EventClientWrapper(configuration, _method);
        }
    }
}
