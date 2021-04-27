using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Handlers;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;

namespace Hades.Transport.Channels.Internal.Server.Channels
{
    internal abstract class ServerChannelBase<THandler, TInvoker, TDataObservable, TDataWriter> :
        HadesChannelBase, IHadesServerChannel<THandler>
        where THandler : IHadesChannelHandler
        where TInvoker : IHadesInvoker
        where TDataObservable : IEndpointObservable
        where TDataWriter : IEndpointWriter
    {
        protected THandler Handler { get; }
        protected IServerEndpointAccessor<TDataObservable, TDataWriter> EndpointAccessor { get; }

        protected ServerChannelBase(
            THandler handler,
            IServerEndpointAccessor<TDataObservable, TDataWriter> endpointAccessor)
        {
            EndpointAccessor = endpointAccessor
                ?? throw new ArgumentNullException(nameof(endpointAccessor));

            Handler = handler;
        }

        protected override void Dispose(bool disposing)
        {
            EndpointAccessor.Dispose();
            base.Dispose(disposing);
        }
    }
}
