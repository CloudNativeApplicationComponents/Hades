using System;

namespace Hades.Core.Internal.Distributions
{
    internal interface ISubscriptionRegistry : IDisposable
    {
        HadesSubscription? Subscription { get; set; }
    }
}
