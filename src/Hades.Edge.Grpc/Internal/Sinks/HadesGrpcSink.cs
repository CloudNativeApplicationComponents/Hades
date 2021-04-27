using CloudNativeApplicationComponents.Utils;
using Hades.Transport.Abstraction;
using Hades.Edge.Abstraction.Features;
using Hades.Edge.Abstraction.Sink;
using System;
using System.Collections.Generic;

namespace Hades.Edge.Grpc.Internal.Sinks
{
    internal class HadesGrpcSink : IHadesSink
    {
        public DataPlane DataPlane => DataPlane.Grpc;

        private readonly KeyedByTypeCollection<IHadesFeature> _features;


        public HadesGrpcSink(IEnumerable<IHadesFeature> hadesFeatures)
        {
            _ = hadesFeatures
                ?? throw new ArgumentNullException(nameof(hadesFeatures));

            _features = new KeyedByTypeCollection<IHadesFeature>(hadesFeatures);
        }

        public TFeature? GetFeature<TFeature>()
            where TFeature : class, IHadesFeature
        {
            return _features.Find<TFeature>();
        }

        public bool TryGetFeature<TFeature>(out TFeature? feature)
            where TFeature : class, IHadesFeature
        {
            return _features.TryGetValue(out feature);
        }
    }
}
