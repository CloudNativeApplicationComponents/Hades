using System;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public interface ISingleEndpointObservable : IEndpointObservable
    {
        IDisposable Subscribe(ISingleEndpointObserver endpointObserver);
    }
}
