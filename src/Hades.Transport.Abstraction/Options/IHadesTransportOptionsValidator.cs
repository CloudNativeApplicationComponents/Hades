namespace Hades.Transport.Abstraction.Options
{
    public interface IHadesTransportOptionsValidator<in T>
        where T : IHadesTransportOptions
    {
        bool Validate(T value);
    }
}
