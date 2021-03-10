using Hades.Core.Abstraction.Distributions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Hades.Core.Internal.Distributions
{
    internal class TopicSubscriptionRegistryGroupCollection
    {
        private readonly ConcurrentDictionary<HadesTopic, SubscriptionRegistryGroupCollection> _items;
        public TopicSubscriptionRegistryGroupCollection()
        {
            _items = new ConcurrentDictionary<HadesTopic, SubscriptionRegistryGroupCollection>();
        }

        public ISubscriptionRegistry CreateRegistry(HadesTopic topic, string group)
        {
            var collection = _items.GetOrAdd(topic, _ => new SubscriptionRegistryGroupCollection());
            return collection.CreateRegistry(group);
        }

        public IEnumerable<HadesBrokerRegistryGroup> Groups(HadesTopic topic)
        {
            if (_items.TryGetValue(topic, out var container))
            {
                return container.Groups();
            }
            return Enumerable.Empty<HadesBrokerRegistryGroup>();
        }

        public HadesBrokerRegistryGroup? Group(HadesTopic topic, string groupName)
        {
            if (_items.TryGetValue(topic, out var container))
            {
                return container.Group(groupName);
            }
            return null;
        }
    }
}
