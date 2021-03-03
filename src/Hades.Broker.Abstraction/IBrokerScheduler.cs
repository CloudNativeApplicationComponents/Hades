using System.Collections.Generic;

namespace Hades.Broker.Abstraction
{
    public interface IBrokerScheduler
    {
        (T Selected, IBrokerSchedulerSelectionMetadata Metadata) Next<T>(
            IEnumerable<T> items, 
            IBrokerSchedulerSelectionMetadata? prevMetadata);
    }
}
