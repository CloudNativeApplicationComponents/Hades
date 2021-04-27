using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Options;
using Hades.Transport.Grpc.Internal.Messages;
using Hades.Transport.Grpc.Internal.Server.EndpointAccessors;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using gc = Grpc.Core;

namespace Hades.Transport.Grpc.Internal.Server
{
    internal class GrpcServerDispatcherAgent : IHadesServerAgent
    {
        private readonly ServerServiceDefinition.Builder _builder;
        private readonly GrpcServerOptions _options;
        private readonly IGrpcMethodFactory _grpcMethodFactory;
        private gc::Server? _server;

        public GrpcServerDispatcherAgent(IHadesServerAgentManager manager,
            IGrpcMethodFactory grpcMethodFactory,
            IOptions<GrpcServerOptions> options)
        {
            _options = options?.Value
                ?? throw new ArgumentNullException(nameof(options));
            _grpcMethodFactory = grpcMethodFactory
                ?? throw new ArgumentNullException(nameof(grpcMethodFactory));

            _builder = new ServerServiceDefinition.Builder();
            manager.AddAgent(this);
        }
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _server = new gc::Server();
            _server.Ports.Add(new ServerPort(_options.Host, _options.Port, _options.Credentials));
            _server.Services.Add(_builder.Build());

            _server.Start();
            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (_server != null)
                await _server.ShutdownAsync();
        }

        public void Dispatch(
            GrpcDuplexStreamingServerEndpointAccessor endpointAccessor,
            GrpcServerEndpointOptions options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var method = _grpcMethodFactory.Create<ProtobufMessage, ProtobufMessage>(
                options.ServiceName,
                options.MethodName,
                MethodType.DuplexStreaming);

            _builder.AddMethod(method, endpointAccessor.CallAsync);
        }

        public void Dispatch(
            GrpcClientStreamingServerEndpointAccessor endpointAccessor,
            GrpcServerEndpointOptions options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var method = _grpcMethodFactory.Create<ProtobufMessage, ProtobufMessage>(
                options.ServiceName,
                options.MethodName,
                MethodType.ClientStreaming);

            _builder.AddMethod(method, endpointAccessor.CallAsync);
        }

        public void Dispatch(
            GrpcServerStreamingServerEndpointAccessor endpointAccessor,
            GrpcServerEndpointOptions options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var method = _grpcMethodFactory.Create<ProtobufMessage, ProtobufMessage>(
                options.ServiceName,
                options.MethodName,
                MethodType.ServerStreaming);

            _builder.AddMethod(method, endpointAccessor.CallAsync);
        }

        public void Dispatch(
            GrpcUnaryServerEndpointAccessor endpointAccessor,
            GrpcServerEndpointOptions options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var method = _grpcMethodFactory.Create<ProtobufMessage, ProtobufMessage>(
                options.ServiceName,
                options.MethodName,
                MethodType.Unary);

            _builder.AddMethod(method, endpointAccessor.CallAsync);
        }

        public void Dispatch(
            GrpcEventServerEndpointAccessor endpointAccessor,
            GrpcServerEndpointOptions options)
        {
            _ = options
                ?? throw new ArgumentNullException(nameof(options));

            var method = _grpcMethodFactory.Create<ProtobufMessage, Empty>(
                options.ServiceName,
                options.MethodName,
                MethodType.Unary);

            _builder.AddMethod(method, endpointAccessor.CallAsync);
        }
    }
}
