using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Hades.Core.Internal.Distributions
{
    internal class SubscriptionRegistryGroupCollection
    {
        private readonly ConcurrentDictionary<string, HadesBrokerRegistryGroup> _groups;

        public SubscriptionRegistryGroupCollection()
        {
            _groups = new();
        }

        public ISubscriptionRegistry CreateRegistry(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException(nameof(groupName));

            var entry = _groups.GetOrAdd(groupName, _ => new HadesBrokerRegistryGroup(groupName));
            lock (entry)
            {
                var registry = new HadesBrokerRegistry(i => entry.Remove(i));
                entry.Add(registry);
                return registry;
            }
        }

        public HadesBrokerRegistryGroup? Group(string groupName)
        {
            if (_groups.TryGetValue(groupName, out var entry))
            {
                return entry;
            }
            return null;
        }

        public IEnumerable<HadesBrokerRegistryGroup> Groups()
        {
            return _groups.Values;
        }

        private class HadesBrokerRegistry : ISubscriptionRegistry, IDisposable
        {
            private readonly Action<HadesBrokerRegistry>? _unsubscribeAction;
            [AllowNull]
            public HadesSubscription? Subscription { get; set; }

            public HadesBrokerRegistry(Action<HadesBrokerRegistry>? unsubscribeAction)
            {
                _unsubscribeAction = unsubscribeAction;
            }

            public void Dispose()
            {
                _unsubscribeAction?.Invoke(this);
                Subscription = null;
                GC.SuppressFinalize(this);
            }
        }
    }
}
