using Hades.Broker.Abstraction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Hades.Broker.Internal
{
    internal class HadesTopicGroupRepository
    {
        private readonly ConcurrentDictionary<string, SubscriptionEntryPool> _repository;
        private readonly HadesTopic _topic;

        public IBrokerScheduler? Scheduler { get; internal set; }

        public HadesTopicGroupRepository(HadesTopic topic)
        {
            _topic = topic
                ?? throw new ArgumentNullException(nameof(topic));

            _repository = new ConcurrentDictionary<string, SubscriptionEntryPool>();
        }

        public SubscriptionEntry AddSubscriptionEntry(string groupName, IHadesConsumer consumer)
        {
            var pool = _repository.GetOrAdd(groupName, _ => new SubscriptionEntryPool(groupName));
            lock (pool)
            {
                var item = new SubscriptionEntry(_topic, groupName, consumer, pool);
                return item;
            }
        }
        public IEnumerable<SubscriptionEntryPool> GetPools()
        {
            return _repository.Values;
        }
        public SubscriptionEntryPool? GetPool(string groupName)
        {
            if (_repository.TryGetValue(groupName, out var pool))
                return pool;
            return null;
        }
    }
}
