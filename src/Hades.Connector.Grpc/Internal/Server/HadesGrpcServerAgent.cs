using Hades.Core.Abstraction.Protocols;
using Spear.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hades.Connector.Grpc.Internal.Server
{
    internal class HadesGrpcServerAgent : IHadesAgent
    {
        private readonly ISpearClient _spearClient;
        private readonly ServerServiceDefinitionBuilder _serverServiceDefinitionBuilder;

        public HadesGrpcServerAgent(ISpearClient spearClient,
            ServerServiceDefinitionBuilder serverServiceDefinitionBuilder)
        {
            _spearClient = spearClient
                ?? throw new ArgumentNullException(nameof(spearClient));

            _serverServiceDefinitionBuilder = serverServiceDefinitionBuilder
                ?? throw new ArgumentNullException(nameof(serverServiceDefinitionBuilder));
        }

        public async Task Start()
        {
            using (var discovery = _spearClient.Discovery())
            {
              var services =  discovery
                    .DiscoverAllServices().Select(service =>
                    {
                        return _serverServiceDefinitionBuilder.Build(service);
                    });

                //Server server = new Server
                //{
                //    Services = services,
                //    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                //};
                //server.Start();

            }
        }

        public async Task Stop()
        {
            //await _server.ShutdownAsync();
        }
    }
}
