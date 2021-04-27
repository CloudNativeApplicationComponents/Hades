using Hades.Edge.Abstraction;
using System;
using System.Threading.Tasks;


namespace Hades.Edge.Grpc.Internal.Angents
{
    internal class HadesGrpcSinkAgent : IHadesAgent
    {
        private readonly IHadesHub _hadesHub;

        public HadesGrpcSinkAgent(IHadesHub hadesHub)
        {
            _hadesHub = hadesHub
                ?? throw new ArgumentNullException(nameof(hadesHub));
        }

        public async Task Start()
        {
            throw new NotImplementedException();
            //TODO Grpc must be constant here!!
            //var sink = _hadesHub.Sink(DataPlane.Grpc);
            //var dynamicServiceFeature = sink.GetFeature<IDynamicServiceFeature>();
            //_serviceDiscovery.GetDynamicServices("Grpc")
            //    .GroupBy(t => t.CatalogName)
            //    .Select(_serverServiceDefinitionBuilder.Build)
            //    .Foreach(_server.Services.Add);

            //await Task.CompletedTask;
        }

        public async Task Stop()
        {
            await Task.CompletedTask;
        }
    }
}
