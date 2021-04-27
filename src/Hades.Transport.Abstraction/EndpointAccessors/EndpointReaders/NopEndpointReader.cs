using System;

namespace Hades.Transport.Abstraction.EndpointAccessors
{
    public sealed class NopEndpointReader : IEndpointReader
    {
        public static NopEndpointReader Nop { get; } = new NopEndpointReader();
        private NopEndpointReader()
        {
        }
        void IDisposable.Dispose()
        {
        }
    }
}
