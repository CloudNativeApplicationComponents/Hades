using Hades.Edge.Abstraction;
using Hades.Edge.Abstraction.Features;
using Hades.Transport.Abstraction;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Options;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;


namespace Hades.Edge.Grpc.Internal.Angents
{
    internal class HadesGrpcConnectorAgent : IHadesAgent
    {
        private readonly IHadesHub _hadesHub;
        private readonly IGrpcTransportFactory _factory;
        private CompositeDisposable _disposable;

        public HadesGrpcConnectorAgent(IHadesHub hadesHub,
            IGrpcTransportFactory factory)
        {
            _factory = factory
                ?? throw new ArgumentNullException(nameof(factory));
            _hadesHub = hadesHub
                ?? throw new ArgumentNullException(nameof(hadesHub));
            _disposable = new CompositeDisposable();
        }

        public async Task Start()
        {
            var connector = _hadesHub.Connector(DataPlane.Grpc);

            var dynamicServiceFeature = connector.GetFeature<IDynamicServiceFeature>();
            foreach (var service in dynamicServiceFeature.GetDynamicServices())
            {
                var option = new GrpcServerEndpointOptions()
                {
                    ServiceName = service.CatalogName,
                    MethodName = service.CatalogName,
                };
                var server = _factory.CreateServer(option);
                _disposable.Add(server);
            }

            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            _disposable.Dispose();
            _disposable = new CompositeDisposable();
            await Task.CompletedTask;
        }
    }
}
