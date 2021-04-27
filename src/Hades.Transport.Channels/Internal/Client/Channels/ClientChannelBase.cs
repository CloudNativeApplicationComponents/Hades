using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using Hades.Transport.Channels.Abstraction;
using Hades.Transport.Channels.Abstraction.Invokers;
using System;

namespace Hades.Transport.Channels.Internal.Client.Channels
{
    internal abstract class ClientChannelBase<TInvoker, TEndpointReader, TEndpointWriter> : HadesChannelBase, IHadesClientChannel<TInvoker>
        where TInvoker : IHadesInvoker
        where TEndpointReader : IEndpointReader
        where TEndpointWriter : IEndpointWriter
    {
        protected IClientEndpointAccessor<TEndpointReader, TEndpointWriter> EndpointAccessor { get; }

        protected ClientChannelBase(IClientEndpointAccessor<TEndpointReader, TEndpointWriter> endpointAccessor)

        {
            EndpointAccessor = endpointAccessor
                ?? throw new ArgumentNullException(nameof(endpointAccessor));
        }

        protected override void Dispose(bool disposing)
        {
            EndpointAccessor.Dispose();
            base.Dispose(disposing);
        }
        public abstract TInvoker CreateInvoker();
    }
}
