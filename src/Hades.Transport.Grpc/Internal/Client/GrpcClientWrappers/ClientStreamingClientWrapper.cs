using Google.Protobuf;
using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using System;

namespace Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers
{
    internal class ClientStreamingClientWrapper : ClientBase<ClientStreamingClientWrapper>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public ClientStreamingClientWrapper(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected ClientStreamingClientWrapper(ClientBaseConfiguration configuration,
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

        protected override ClientStreamingClientWrapper NewInstance(ClientBaseConfiguration configuration)
        {
            return new ClientStreamingClientWrapper(configuration, _method);
        }
    }
}
