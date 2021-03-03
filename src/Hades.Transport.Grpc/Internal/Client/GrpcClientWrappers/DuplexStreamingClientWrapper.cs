using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using System;

namespace Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers
{
    internal class DuplexStreamingClientWrapper : ClientBase<DuplexStreamingClientWrapper>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;
        public DuplexStreamingClientWrapper(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected DuplexStreamingClientWrapper(ClientBaseConfiguration configuration,
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

        protected override DuplexStreamingClientWrapper NewInstance(ClientBaseConfiguration configuration)
        {
            return new DuplexStreamingClientWrapper(configuration, _method);
        }
    }
}
