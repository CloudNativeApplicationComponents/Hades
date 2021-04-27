using Hades.Transport.Abstraction.Client;
using Hades.Transport.Abstraction.EndpointAccessors;
using System;

namespace Hades.Transport.Internal.Client
{
    internal class ClientEndpointAccessor<TEndpointReader, TEndpointWriter> : IClientEndpointAccessor<TEndpointReader, TEndpointWriter>
        where TEndpointReader : IEndpointReader
        where TEndpointWriter : IEndpointWriter
    {
        public TEndpointWriter Writer { get; }

        public TEndpointReader Reader { get; }

        public ClientEndpointAccessor(TEndpointReader reader, TEndpointWriter writer)
        {
            Reader = reader
                ?? throw new ArgumentNullException(nameof(reader));
            Writer = writer
                ?? throw new ArgumentNullException(nameof(writer));
        }

        public void Dispose()
        {
            Reader.Dispose();
            Writer.Dispose();
        }
    }
}
