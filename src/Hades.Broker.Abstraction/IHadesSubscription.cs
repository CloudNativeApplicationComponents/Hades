using System;

namespace Hades.Broker.Abstraction
{
    public interface IHadesSubscription : IDisposable
    {
        Guid SubscriptionId { get; }
    }
}
