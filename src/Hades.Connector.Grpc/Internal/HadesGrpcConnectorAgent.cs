using CloudNativeApplicationComponents.Utils;
using Grpc.Core;
using Hades.Connector.Grpc.Internal.Options;
using Hades.Core.Abstraction.Protocols;
using Microsoft.Extensions.Options;
using Spear.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Hades.Connector.Grpc.Internal
{
    internal class HadesGrpcConnectorAgent : IHadesAgent
    {
        private readonly ISpearClient _spearClient;
        private readonly ServerServiceDefinitionBuilder _serverServiceDefinitionBuilder;
        private readonly HadesGrpcConnectorOption _option;
        private readonly Server _server;

        public HadesGrpcConnectorAgent(ISpearClient spearClient,
            ServerServiceDefinitionBuilder serverServiceDefinitionBuilder,
            IOptions<HadesGrpcConnectorOption> option)
        {
            _spearClient = spearClient
                ?? throw new ArgumentNullException(nameof(spearClient));

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
            _spearClient.Discovery().DiscoverAllServices()
                .Select(service => _serverServiceDefinitionBuilder.Build(service))
                .Foreach(t => _server.Services.Add(t));

            _server.Start();
            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            await _server.ShutdownAsync();
        }
    }
}
