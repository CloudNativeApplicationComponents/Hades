using Hades.Broker.Abstraction;
using System;

namespace Hades.Broker.Internal
{
    internal class HadesSubscription : IHadesSubscription
    {
        public Guid SubscriptionId { get; }
        public HadesTopic Topic { get; }
        public string GroupName { get; }

        private readonly IDisposable _disposable;
        public HadesSubscription(HadesTopic topic, string groupName, IDisposable disposable)
        {
            Topic = topic
                ?? throw new ArgumentNullException(nameof(topic));
            GroupName = groupName
                ?? throw new ArgumentNullException(nameof(groupName));
            _disposable = disposable;
            SubscriptionId = Guid.NewGuid();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
