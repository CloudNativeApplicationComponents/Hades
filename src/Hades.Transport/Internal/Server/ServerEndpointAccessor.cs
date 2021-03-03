using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using System;

namespace Hades.Transport.Internal.Server
{
    internal class ServerEndpointAccessor<TEndpointObservable, TEndpointWriter> : 
        IServerEndpointAccessor<TEndpointObservable, TEndpointWriter>
        where TEndpointObservable : IEndpointObservable
        where TEndpointWriter : IEndpointWriter
    {
        public TEndpointWriter Writer { get; }

        public TEndpointObservable Observable { get; }

        public ServerEndpointAccessor(TEndpointObservable observable, TEndpointWriter writer)
        {
            Observable = observable
                ?? throw new ArgumentNullException(nameof(observable));
            Writer = writer
                ?? throw new ArgumentNullException(nameof(writer));
        }

        public void Dispose()
        {
            Observable.Dispose();
            Writer.Dispose();
        }
    }
}
