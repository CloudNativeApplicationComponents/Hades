using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using System;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class DuplexStreamingClient : ClientBase<DuplexStreamingClient>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public DuplexStreamingClient(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected DuplexStreamingClient(ClientBaseConfiguration configuration,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(configuration)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        public virtual AsyncDuplexStreamingCall<ProtobufMessage, ProtobufMessage> Send(CallOptions options)
        {
            return CallInvoker.AsyncDuplexStreamingCall(_method, null, options);
        }

        protected override DuplexStreamingClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new DuplexStreamingClient(configuration, _method);
        }
    }
}
