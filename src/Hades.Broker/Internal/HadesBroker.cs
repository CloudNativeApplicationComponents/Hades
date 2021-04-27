using Hades.Broker.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Broker.Internal
{
    internal class HadesBroker : IHadesBroker
    {
        private readonly HadesTopicRepository _repository;
        private readonly IBrokerScheduler _defaultScheduler;

        public HadesBroker(IBrokerScheduler defaultScheduler)
        {
            _defaultScheduler = defaultScheduler
                ?? throw new ArgumentNullException(nameof(defaultScheduler));

            _repository = new HadesTopicRepository();
        }

        public async Task<DeliveryResult> PublishAsync(HadesTopic topic,
            IHadesMessage message,
            CancellationToken cancellationToken = default)
        {
            _ = message
                ?? throw new ArgumentNullException(nameof(message));
            _ = topic
                ?? throw new ArgumentNullException(nameof(topic));

            //TODO logically must review
            var groups = _repository.GetGroups(topic);
            if (groups == null)
                return DeliveryResult.NotDelivered;

            foreach (var pool in groups.GetPools())
            {
                var entries = pool.GetEntries();
                var scheduler = groups.Scheduler ?? _defaultScheduler;
                SubscriptionEntry entry;
                lock (pool)
                {
                    (entry, pool.SchedulerMetadata) = scheduler.Next(entries, pool.SchedulerMetadata);
                }
                await entry.ScheduleAsync(message, cancellationToken);
            }
            return DeliveryResult.Delivered;
        }

        public void Scheduler(HadesTopic topic, IBrokerScheduler? scheduler)
        {
            _ = scheduler
                ?? throw new ArgumentNullException(nameof(scheduler));
            _ = topic
                ?? throw new ArgumentNullException(nameof(topic));

            var groups = _repository.GetGroups(topic);
            if (groups != null)
                groups.Scheduler = scheduler ?? _defaultScheduler;
        }

        public async Task<DeliveryResult> Send(
            IHadesSubscription subscription,
            IHadesMessage message,
            CancellationToken cancellationToken = default)
        {
            _ = message
                ?? throw new ArgumentNullException(nameof(message));
            _ = subscription
                ?? throw new ArgumentNullException(nameof(subscription));

            if (subscription is HadesSubscription hadesSubscription)
            {
                var entry = _repository.GetEntry(hadesSubscription);
                if (entry == null)
                    throw new InvalidOperationException("Could not find subscription.");
                await entry.ScheduleAsync(message, cancellationToken);
                return DeliveryResult.Delivered;
            }
            else
            {
                throw new ArgumentException("Invalid subscription", nameof(subscription));
            }
        }

        public IHadesSubscription Subscribe(
            HadesTopic topic,
            string groupName,
            IHadesConsumer consumer)
        {
            _ = topic
                ?? throw new ArgumentNullException(nameof(topic));
            _ = consumer
                ?? throw new ArgumentNullException(nameof(consumer));
            if(string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException(nameof(groupName));

            var entry = _repository.AddSubscriptionEntry(topic, groupName, consumer);
            return entry.Subscription;
        }
    }
}
