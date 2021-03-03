using Grpc.Core;
using Hades.Transport.Grpc.Internal.Messages;
using System;

namespace Hades.Transport.Grpc.Internal.Client.GrpcClientWrappers
{
    internal class UnaryClientWrapper : ClientBase<UnaryClientWrapper>
    {
        private readonly Method<ProtobufMessage, ProtobufMessage> _method;

        public UnaryClientWrapper(ChannelBase channel,
            Method<ProtobufMessage, ProtobufMessage> method)
            : base(channel)
        {
            _method = method
                ?? throw new ArgumentNullException(nameof(method));
        }

        protected UnaryClientWrapper(ClientBaseConfiguration configuration,
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

        protected override UnaryClientWrapper NewInstance(ClientBaseConfiguration configuration)
        {
            return new UnaryClientWrapper(configuration, _method);
        }
    }
}
