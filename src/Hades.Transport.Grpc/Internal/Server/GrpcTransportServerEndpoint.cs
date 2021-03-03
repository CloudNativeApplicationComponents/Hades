using Hades.Transport.Abstraction;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Abstraction.Server;
using Hades.Transport.Grpc.Internal.Serialization;
using Hades.Transport.Grpc.Internal.Server.EndpointAccessors;
using Hades.Transport.Internal.Server;
using System;

namespace Hades.Transport.Grpc.Internal.Server
{
    internal class GrpcTransportServerEndpoint :
        HadesTransportServerEndpointBase<GrpcServerEndpointOptions>,
        IGrpcTransportServerEndpoint,
        IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>,
        IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>,
        IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>
    {
        private readonly GrpcServerDispatcherAgent _dispatcher;
        private readonly IProtobufEnvelopeSerializer _serializer;
        private readonly IProtobufEnvelopeDeserializer _deserializer;

        public GrpcTransportServerEndpoint(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer,
            GrpcServerDispatcherAgent dispatcher,
            GrpcServerEndpointOptions options)
            : base(options)
        {
            _dispatcher = dispatcher
                ?? throw new ArgumentNullException(nameof(dispatcher));
            _serializer = serializer ??
                throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ??
                throw new ArgumentNullException(nameof(deserializer));

        }

        public override DataPlane DataPlane { get => DataPlane.Grpc; }

        public IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter> ClientStreaming()
        {
            var pipe = new GrpcClientStreamingServerEndpointAccessor(_serializer, _deserializer);
            _dispatcher.Dispatch(pipe, Options);
            return pipe;
        }

        public IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter> DuplexStreaming()
        {
            var pipe = new GrpcDuplexStreamingServerEndpointAccessor(_serializer, _deserializer);
            _dispatcher.Dispatch(pipe, Options);
            return pipe;
        }

        public IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter> Event()
        {
            var pipe = new GrpcEventServerEndpointAccessor(_serializer, _deserializer);
            _dispatcher.Dispatch(pipe, Options);
            return pipe;
        }

        public IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter> ServerStreaming()
        {
            var pipe = new GrpcServerStreamingServerEndpointAccessor(_serializer, _deserializer);
            _dispatcher.Dispatch(pipe, Options);
            return pipe;
        }

        public IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter> Unary()
        {
            var pipe = new GrpcUnaryServerEndpointAccessor(_serializer, _deserializer);
            _dispatcher.Dispatch(pipe, Options);
            return pipe;
        }


        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter> IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeSingleEndpointWriter>.Create()
        {
            return ClientStreaming();
        }

        IServerEndpointAccessor<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter> IServerEndpointAccessorFactory<IStreamEndpointObservable, ICorrelativeStreamEndpointWriter>.Create()
        {
            return DuplexStreaming();
        }

        IServerEndpointAccessor<ISingleEndpointObservable, NopEndpointWriter> IServerEndpointAccessorFactory<ISingleEndpointObservable, NopEndpointWriter>.Create()
        {
            return Event();
        }

        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter> IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeStreamEndpointWriter>.Create()
        {
            return ServerStreaming();
        }

        IServerEndpointAccessor<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter> IServerEndpointAccessorFactory<ISingleEndpointObservable, ICorrelativeSingleEndpointWriter>.Create()
        {
            return Unary();
        }
    }
}
