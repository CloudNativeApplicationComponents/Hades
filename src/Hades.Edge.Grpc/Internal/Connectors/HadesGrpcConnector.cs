using CloudNativeApplicationComponents.Utils;
using Hades.Edge.Abstraction;
using Hades.Edge.Abstraction.Connectors;
using Hades.Edge.Abstraction.Features;
using Hades.Edge.Grpc.Internal.Angents;
using Hades.Edge.Grpc.Internal.Features;
using Hades.Transport.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Hades.Edge.Grpc.Internal.Connectors
{
    internal class HadesGrpcConnector : IHadesConnector
    {
        public DataPlane DataPlane => DataPlane.Grpc;
        private readonly KeyedByTypeCollection<IHadesFeature> _features;

        public HadesGrpcConnector(
            IServiceProvider serviceProvider)
        {
            _features = new KeyedByTypeCollection<IHadesFeature>();
            _features.Add(serviceProvider.GetRequiredService<GrpcDynamicServiceFeature>());
        }

        public TFeature? GetFeature<TFeature>()
            where TFeature : class, IHadesFeature
        {
            return _features.Find<TFeature>();
        }

        public void Start(ExecutionContext context)
        {
            context.Agents.Add(typeof(HadesGrpcConnectorAgent));
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public bool TryGetFeature<TFeature>(out TFeature? feature)
            where TFeature : class, IHadesFeature
        {
            return _features.TryGetValue(out feature);
        }
    }
}
