namespace Hades.Edge.Abstraction.Features
{
    public interface IFeaatureContainer
    {
        bool TryGetFeature<TFeature>(out TFeature? feature)
            where TFeature : class, IHadesFeature;
    }
}
