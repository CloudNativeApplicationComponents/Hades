using Hades.Transport.Abstraction.EndpointAccessors;
using System;

namespace Hades.Transport.Abstraction.Client
{
    public interface IClientEndpointAccessor<out TEndpointReader, out TEndpointWriter> : IDisposable
        where TEndpointReader : IEndpointReader
        where TEndpointWriter : IEndpointWriter
    {
        public TEndpointWriter Writer { get; }
        public TEndpointReader Reader { get; }
    }
}
