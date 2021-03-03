using Hades.Broker.Abstraction;
using System.Collections.Concurrent;
using System.Linq;

namespace Hades.Broker.Internal
{
    internal class HadesTopicRepository
    {
        private readonly ConcurrentDictionary<HadesTopic, HadesTopicGroupRepository> _repository;
        public HadesTopicRepository()
        {
            _repository = new ConcurrentDictionary<HadesTopic, HadesTopicGroupRepository>();
        }
        public SubscriptionEntry AddSubscriptionEntry(HadesTopic topic, string groupName, IHadesConsumer consumer)
        {
            var items = _repository.GetOrAdd(topic, _ => new HadesTopicGroupRepository(topic));
            return items.AddSubscriptionEntry(groupName, consumer);
        }

        public HadesTopicGroupRepository? GetGroups(HadesTopic topic)
        {
            if (_repository.TryGetValue(topic, out var items))
            {
                return items;
            }
            return null;
        }

        public SubscriptionEntry? GetEntry(HadesSubscription hadesSubscription)
        {
            return GetGroups(hadesSubscription.Topic)?
                .GetPool(hadesSubscription.GroupName)?
                .GetEntries()
                .FirstOrDefault(t=>t.Subscription.SubscriptionId == hadesSubscription.SubscriptionId);
        }
    }
}
