using Hades.Core.Abstraction.Distributions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hades.Core.Internal.Distributions
{
    internal class DefaultHadesBroker : IHadesBroker
    {
        private readonly TopicSubscriptionRegistryGroupCollection _repository;

        public DefaultHadesBroker()
        {
            _repository = new TopicSubscriptionRegistryGroupCollection();
        }

        public async Task Publish(Envelope envelope)
        {
                //TODO create pool
            foreach (var group in _repository.Groups(envelope.Topic))
            {
                //TODO round-robin,... scheduling
                var entry = group.Registries.FirstOrDefault();
                if (entry != null && entry.Subscription != null)
                {
                    await entry.Subscription.HadesConsumer.Consume(envelope);
                }
            }
        }

        public IHadesSubscription Subscribe(IHadesConsumer consumer, HadesTopic topic, string group)
        {
            var entry = _repository.CreateRegistry(topic, group);
            var subscription = CreateSubscription(consumer, new HadesUnsubscribeHandler(entry));
            return subscription;
        }

        private HadesSubscription CreateSubscription(IHadesConsumer consumer,
            IHadesUnsubscribeHandler handler)
        {
            return new HadesSubscription(consumer, handler);
        }

        private class HadesUnsubscribeHandler : IHadesUnsubscribeHandler
        {
            private ISubscriptionRegistry _entry;
            public HadesUnsubscribeHandler(ISubscriptionRegistry entry)
            {
                _entry = entry
                    ?? throw new ArgumentNullException(nameof(entry));
            }

            public void Dispose()
            {
                _entry.Dispose();

                GC.SuppressFinalize(this);
            }
        }
    }
}
