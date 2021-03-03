using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using System;

namespace Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers
{
    internal class ServerStreamingClientWrapper : ClientBase<ServerStreamingClientWrapper>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public ServerStreamingClientWrapper(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected ServerStreamingClientWrapper(ClientBaseConfiguration configuration,
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

        protected override ServerStreamingClientWrapper NewInstance(ClientBaseConfiguration configuration)
        {
            return new ServerStreamingClientWrapper(configuration, _method);
        }
    }
}
