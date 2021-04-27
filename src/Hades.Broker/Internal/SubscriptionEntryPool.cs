using Hades.Broker.Abstraction;
using System.Collections.Generic;

namespace Hades.Broker.Internal
{
    internal class SubscriptionEntryPool
    {
        private readonly List<SubscriptionEntry> _entries;
        public string GroupName { get; }
        public IBrokerSchedulerSelectionMetadata? SchedulerMetadata { get; set; }

        public SubscriptionEntryPool(string groupName)
        {
            GroupName = groupName;
            _entries = new List<SubscriptionEntry>();
        }
        internal void Add(SubscriptionEntry entry)
        {
            _entries.Add(entry);
        }

        internal void Remove(SubscriptionEntry entry)
        {
            _entries.Remove(entry);
        }
        public IEnumerable<SubscriptionEntry> GetEntries()
        {
            return _entries;
        }
    }
}
