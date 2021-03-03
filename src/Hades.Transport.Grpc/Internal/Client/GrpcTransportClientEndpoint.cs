using Hades.Transport.Abstraction;
using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Client;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Internal.Client.EndpointAccessors;
using Hades.Transport.Grpc.Internal.Serialization;
using Hades.Transport.Internal.Client;
using System;

namespace Hades.Transport.Grpc.Internal.Client
{
    internal class GrpcTransportClientEndpoint :
        HadesTransportClientEndpointBase<GrpcClientEndpointOptions>,
        IGrpcTransportClientEndpoint,
        IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>,
        IClientEndpointAccessorFactory<NopEndpointReader, ISingleEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>,
        IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>
    {
        private readonly IGrpcMethodFactory _grpcMethodFactory;
        private readonly IProtobufEnvelopeSerializer _serializer;
        private readonly IProtobufEnvelopeDeserializer _deserializer;

        public GrpcTransportClientEndpoint(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            IGrpcMethodFactory grpcMethodFactory,
            GrpcClientEndpointOptions options)
            : base(options)
        {
            _serializer = serializer ??
                throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ??
                throw new ArgumentNullException(nameof(deserializer));
            _grpcMethodFactory = grpcMethodFactory
                ?? throw new ArgumentNullException(nameof(grpcMethodFactory));
        }

        public override DataPlane DataPlane { get => DataPlane.Grpc; }

        public IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter> ClientStreaming()
        {
            return new GrpcClientStreamingClientEndpointAccessor(_serializer, _deserializer, _grpcMethodFactory, Options);
        }

        public IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter> DuplexStreaming()
        {
            return new GrpcDuplexStreamingClientEndpointAccessor(_serializer, _deserializer, _grpcMethodFactory, Options);
        }

        public IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter> Event()
        {
            return new GrpcEventClientEndpointAccessor(_serializer, _deserializer, _grpcMethodFactory, Options);
        }

        public IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter> ServerStreaming()
        {
            return new GrpcServerStreamingClientEndpointAccessor(_serializer, _deserializer, _grpcMethodFactory, Options);
        }

        public IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter> Unary()
        {
            return new GrpcUnaryClientEndpointAccessor(_serializer, _deserializer, _grpcMethodFactory, Options);
        }


        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, IStreamEndpointWriter> IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, IStreamEndpointWriter>.Create()
        {
            return ClientStreaming();
        }

        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, IStreamEndpointWriter> IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, IStreamEndpointWriter>.Create()
        {
            return DuplexStreaming();
        }

        IClientEndpointAccessor<NopEndpointReader, ISingleEndpointWriter> IClientEndpointAccessorFactory<NopEndpointReader, ISingleEndpointWriter>.Create()
        {
            return Event();
        }

        IClientEndpointAccessor<ICorrelativeStreamEndpointReader, ISingleEndpointWriter> IClientEndpointAccessorFactory<ICorrelativeStreamEndpointReader, ISingleEndpointWriter>.Create()
        {
            return ServerStreaming();
        }

        IClientEndpointAccessor<ICorrelativeSingleEndpointReader, ISingleEndpointWriter> IClientEndpointAccessorFactory<ICorrelativeSingleEndpointReader, ISingleEndpointWriter>.Create()
        {
            return Unary();
        }
    }
}
