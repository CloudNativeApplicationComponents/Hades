using Grpc.Core;
using Hades.Edge.Grpc.Internal.Messages;
using System;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class ServerStreamingClient : ClientBase<ServerStreamingClient>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public ServerStreamingClient(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected ServerStreamingClient(ClientBaseConfiguration configuration,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(configuration)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        public virtual AsyncServerStreamingCall<ProtobufMessage> Send(ProtobufMessage request,
            CallOptions options)
        {
            return CallInvoker.AsyncServerStreamingCall(_method, null, options, request);
        }

        protected override ServerStreamingClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new ServerStreamingClient(configuration, _method);
        }
    }
}
