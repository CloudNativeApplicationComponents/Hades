using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction.ChannelFactories
{
    public interface IHadesServerChannelFactory<TChannel, in THandler, in TEndpointObservable, in TEndpointWriter>
        where TChannel : IHadesServerChannel<THandler>
        where THandler : IHadesChannelHandler
        where TEndpointObservable : IEndpointObservable
        where TEndpointWriter : IEndpointWriter
    {
        TChannel Create(
            THandler handler,
            IServerEndpointAccessor<TEndpointObservable, TEndpointWriter> accessor);
    }
}
