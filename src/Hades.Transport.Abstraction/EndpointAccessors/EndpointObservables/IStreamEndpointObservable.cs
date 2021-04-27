using System;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface IStreamEndpointObservable : IEndpointObservable
    {
        IDisposable Subscribe(IStreamEndpointObserver endpointObserver);
    }
}
