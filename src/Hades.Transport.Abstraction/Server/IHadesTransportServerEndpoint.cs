using System;

namespace Hades.Transport.Abstraction.Server
{
    public interface IHadesTransportServerEndpoint : IDisposable
    {
        DataPlane DataPlane { get; }
    }
    public interface IHadesTransportServerEndpoint<TOptions> : IHadesTransportServerEndpoint
    {
    }
}
