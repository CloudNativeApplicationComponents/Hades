using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using System;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class ClientStreamingClient : ClientBase<ClientStreamingClient>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public ClientStreamingClient(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected ClientStreamingClient(ClientBaseConfiguration configuration,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(configuration)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        public virtual AsyncClientStreamingCall<ProtobufMessage, ProtobufMessage> Send(CallOptions options)
        {
            return CallInvoker.AsyncClientStreamingCall(_method, null, options);
        }

        protected override ClientStreamingClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new ClientStreamingClient(configuration, _method);
        }
    }
}
