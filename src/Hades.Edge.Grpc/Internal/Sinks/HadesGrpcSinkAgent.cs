using CloudNativeApplicationComponents.Utils;
using Grpc.Core;
using Hades.Core.Abstraction.Protocols;
using Hades.Edge.Abstraction.Services;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Hades.Edge.Grpc.Internal.Connectors
{
    internal class HadesGrpcSinkAgent : IHadesAgent
    {
        private readonly IDynamicServiceDiscovery _serviceDiscovery;

        public HadesGrpcSinkAgent(IDynamicServiceDiscovery serviceDiscovery)
        {
            _serviceDiscovery = serviceDiscovery
                ?? throw new ArgumentNullException(nameof(serviceDiscovery));
        }

        public async Task Start()
        {
            //TODO Grpc must be constant here!!
            //_serviceDiscovery.GetDynamicServices("Grpc")
            //    .GroupBy(t => t.CatalogName)
            //    .Select(_serverServiceDefinitionBuilder.Build)
            //    .Foreach(_server.Services.Add);

            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            await Task.CompletedTask;
        }
    }
}
