using Hades.Transport.Abstraction;
using Hades.Transport.Abstraction.Server;
using System;

namespace Hades.Transport.Internal.Server
{
    internal abstract class HadesTransportServerEndpointBase<TOptions> : IHadesTransportServerEndpoint<TOptions>
    {
        private bool _disposedValue;

        public TOptions Options { get; }

        protected HadesTransportServerEndpointBase(TOptions options)
        {
            Options = options
                ?? throw new ArgumentNullException(nameof(options));

        }

        public abstract DataPlane DataPlane { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
