using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using System;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class UnaryClient : ClientBase<UnaryClient>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public UnaryClient(ChannelBase channel, 
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected UnaryClient(ClientBaseConfiguration configuration, 
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

        protected override UnaryClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new UnaryClient(configuration, _method);
        }
    }
}
