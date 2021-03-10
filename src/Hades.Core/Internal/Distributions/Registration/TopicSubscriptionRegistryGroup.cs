using Hades.Core.Abstraction.Distributions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hades.Core.Internal.Distributions
{
    internal class TopicSubscriptionRegistryGroup
    {
        private readonly IList<HadesBrokerRegistryGroup> _subsciptionGroups;

        public HadesTopic Topic { get; }
        public IEnumerable<HadesBrokerRegistryGroup> SubsciptionGroups
        {
            get => new ReadOnlyCollection<HadesBrokerRegistryGroup>(_subsciptionGroups);
        }

        public TopicSubscriptionRegistryGroup(HadesTopic topic, IList<HadesBrokerRegistryGroup> subsciptionGroups)
        {
            Topic = topic
                ?? throw new ArgumentNullException(nameof(topic));
            _subsciptionGroups = subsciptionGroups
                ?? throw new ArgumentNullException(nameof(subsciptionGroups));
        }
    }
}
