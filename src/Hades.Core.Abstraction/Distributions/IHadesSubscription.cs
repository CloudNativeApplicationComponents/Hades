using System;

namespace Hades.Core.Abstraction.Distributions
{
    public interface IHadesSubscription : IDisposable
    {
        Guid SubscriptionId { get; }
    }
}
