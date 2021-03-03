using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Client;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Abstraction.Server;
using Hades.Transport.Grpc.Internal.Client;
using Hades.Transport.Grpc.Internal.Serialization;
using Hades.Transport.Grpc.Internal.Server;
using System;

namespace Hades.Transport.Grpc.Internal
{
    internal class GrpcTransportFactory : IGrpcTransportFactory
    {
        private readonly GrpcServerDispatcherAgent _dispatcher;
        private readonly IGrpcMethodFactory _grpcMethodFactory;
        private readonly IProtobufEnvelopeSerializer _serializer;
        private readonly IProtobufEnvelopeDeserializer _deserializer;

        public GrpcTransportFactory(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcServerDispatcherAgent dispatcher)
        {
            _dispatcher = dispatcher
                ?? throw new ArgumentNullException(nameof(dispatcher));
            _grpcMethodFactory = grpcMethodFactory
                ?? throw new ArgumentNullException(nameof(grpcMethodFactory));
            _serializer = serializer ??
                throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ??
                throw new ArgumentNullException(nameof(deserializer));
        }

        public IGrpcTransportClientEndpoint CreateClient(GrpcClientEndpointOptions options)
        {
            return new GrpcTransportClientEndpoint(
                _serializer,
                _deserializer,
                _grpcMethodFactory,
                options);
        }

        public IGrpcTransportServerEndpoint CreateServer(GrpcServerEndpointOptions options)
        {
            return new GrpcTransportServerEndpoint(
                _serializer,
                _deserializer,
                _dispatcher,
                options);
        }
    }
}
