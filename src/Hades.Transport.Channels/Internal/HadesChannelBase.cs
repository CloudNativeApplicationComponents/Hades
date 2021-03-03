using Hades.Transport.Channels.Abstraction;
using System;

namespace Hades.Transport.Channels.Internal
{
    internal abstract class HadesChannelBase : IHadesChannel, IDisposable
    {
        private bool _disposedValue;
        protected HadesChannelBase()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }
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
