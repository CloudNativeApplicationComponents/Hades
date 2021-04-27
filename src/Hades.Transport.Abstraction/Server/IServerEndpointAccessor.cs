using Hades.Transport.Abstraction.EndpointAccessors;
using System;

namespace Hades.Transport.Abstraction.Server
{
    public interface IServerEndpointAccessor<out TEndpointObservable, out TEndpointWriter> : IDisposable
        where TEndpointObservable : IEndpointObservable
        where TEndpointWriter : IEndpointWriter
    {
        TEndpointObservable Observable { get; }
        TEndpointWriter Writer { get; }
    }
}
