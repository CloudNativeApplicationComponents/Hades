using System;

namespace Hades.Transport.Abstraction.Client
{
    public interface IHadesTransportClientEndpoint : IDisposable
    {
        DataPlane DataPlane { get; }
    }

    public interface IHadesTransportClientEndpoint<TOptions> : IHadesTransportClientEndpoint
    {
    }
}
