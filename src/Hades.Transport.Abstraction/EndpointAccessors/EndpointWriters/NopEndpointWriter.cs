using System;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public sealed class NopEndpointWriter : IEndpointWriter
    {
        public static NopEndpointWriter Nop { get; } = new NopEndpointWriter();
        private NopEndpointWriter()
        {
        }
        void IDisposable.Dispose()
        {
        }
    }
}
