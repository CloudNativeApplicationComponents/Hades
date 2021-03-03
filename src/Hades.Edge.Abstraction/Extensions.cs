using Hades.Edge.Abstraction.Features;
using System;

namespace Hades.Edge.Abstraction
{
    public static class Extensions
    {
        public static TFeature GetFeature<TFeature>(this IFeaatureContainer feaatureContainer)
            where TFeature : class, IHadesFeature
        {
            if(feaatureContainer.TryGetFeature<TFeature>(out var feature))
            {
                return feature!;
            }
            throw new InvalidOperationException($"Container contains no feature with type = `{typeof(TFeature).FullName}`.");
        }
    }
}
