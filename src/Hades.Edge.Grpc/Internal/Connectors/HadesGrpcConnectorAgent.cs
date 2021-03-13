using CloudNativeApplicationComponents.Utils;
using Grpc.Core;
using Hades.Core.Abstraction.Protocols;
using Hades.Edge.Abstraction.Services;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Hades.Edge.Grpc.Internal.Connectors
{
    internal class HadesGrpcConnectorAgent : IHadesAgent
    {
        private readonly IDynamicServiceDiscovery _serviceDiscovery;
        private readonly ServerServiceDefinitionBuilder _serverServiceDefinitionBuilder;
        private readonly HadesGrpcConnectorOption _option;
        private readonly Server _server;

        public HadesGrpcConnectorAgent(IDynamicServiceDiscovery serviceDiscovery,
            ServerServiceDefinitionBuilder serverServiceDefinitionBuilder,
            IOptions<HadesGrpcConnectorOption> option)
        {
            _serviceDiscovery = serviceDiscovery
                ?? throw new ArgumentNullException(nameof(serviceDiscovery));

            _serverServiceDefinitionBuilder = serverServiceDefinitionBuilder
                ?? throw new ArgumentNullException(nameof(serverServiceDefinitionBuilder));

            _option = option?.Value
                ?? throw new ArgumentNullException(nameof(option));

            //TODO ServerCredential must be set here!!
            _server = new Server();
            _option.Port.ForEach(t =>
                _server.Ports.Add(new ServerPort(_option.Endpoint.AbsoluteUri, t, ServerCredentials.Insecure)));
        }

        public async Task Start()
        {
            //TODO Grpc must be constant here!!
            (await _serviceDiscovery.GetDynamicServices("Grpc"))
                .GroupBy(t => t.CatalogName)
                .Select(_serverServiceDefinitionBuilder.Build)
                .Foreach(_server.Services.Add);

            _server.Start();
        }

        public async Task Stop()
        {
            await _server.ShutdownAsync();
        }
    }
}
