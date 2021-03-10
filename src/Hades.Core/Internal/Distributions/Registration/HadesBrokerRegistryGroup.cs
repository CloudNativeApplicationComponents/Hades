using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hades.Core.Internal.Distributions
{
    internal class HadesBrokerRegistryGroup
    {
        private readonly List<ISubscriptionRegistry> _registries;
        public string GroupName { get; }
        public IEnumerable<ISubscriptionRegistry> Registries
        {
            get => new ReadOnlyCollection<ISubscriptionRegistry>(_registries);
        }


        public HadesBrokerRegistryGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException(nameof(groupName));

            GroupName = groupName;
            _registries = new List<ISubscriptionRegistry>();
        }

        public virtual void Remove(ISubscriptionRegistry registry)
        {
            _ = registry 
                ?? throw new ArgumentNullException(nameof(registry));

            _registries.Remove(registry);
        }

        public virtual void Add(ISubscriptionRegistry registry)
        {
            _ = registry
                ?? throw new ArgumentNullException(nameof(registry));

            _registries.Add(registry);
        }
    }
}
