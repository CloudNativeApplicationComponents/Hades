using Hades.Core.Abstraction.Distributions;
using System;

namespace Hades.Core.Internal.Distributions
{
    internal class HadesSubscription : IHadesSubscription, IDisposable
    {
        private bool _disposedValue;
        private readonly IHadesUnsubscribeHandler _unsubscribeHandler;

        public IHadesConsumer HadesConsumer { get; }
        public Guid SubscriptionId { get; }


        public HadesSubscription(IHadesConsumer hadesConsumer,
            IHadesUnsubscribeHandler unsubscribeHandler)
        {
            _unsubscribeHandler = unsubscribeHandler
                ?? throw new ArgumentNullException(nameof(unsubscribeHandler));
            HadesConsumer = hadesConsumer
                ?? throw new ArgumentNullException(nameof(hadesConsumer));

            SubscriptionId = Guid.NewGuid();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unsubscribeHandler.Dispose();
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
