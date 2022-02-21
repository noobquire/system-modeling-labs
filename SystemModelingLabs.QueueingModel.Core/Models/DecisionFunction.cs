using SystemModelingLabs.QueueingModel.Core.Elements;

namespace SystemModelingLabs.QueueingModel.Core.Models
{
    public delegate Element<T> DecisionFunction<T>(
            IEnumerable<Element<T>> elements,
            T item
        ) where T : QueueItem;
}
