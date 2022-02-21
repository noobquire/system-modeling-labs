using SystemModelingLabs.QueueingModel.Core.Elements;
using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs
{
    public class QueueingModel<T> where T : QueueItem
    {
        public List<Element<T>> Elements { get; init; }
        public double CurrentTime { get; private set; } = 0;
        public int TotalItemsCreated => Elements.Where(e => e is CreateItemsElement<T>)
                .Sum(e => ((CreateItemsElement<T>)e).ProcessedItemsCount);
        public int TotalFailures => Elements.Where(e => e is ProcessingChannelElement<T>)
                .Sum(e => ((ProcessingChannelElement<T>)e).FailureCount);
        public int TotalQueueChanges => Elements.Where(e => e is ParallelProcessingChannelElement<T>)
                .Sum(e => ((ParallelProcessingChannelElement<T>)e).QueueSwitchCount);
        public int TotalItemsProcessed =>  Elements.Where(e => e is ProcessingChannelElement<T>)
                .Sum(e => ((ProcessingChannelElement<T>)e).ProcessedItemsCount);

        public QueueingModel(IEnumerable<Element<T>> elements,
            int maxQueue = int.MaxValue)
        {
            if (elements == null || !elements.Any())
            {
                throw new ArgumentNullException(nameof(elements), "Queueing model must contain elements");
            }
            this.Elements = elements.ToList();
        }

        public void Simulate(int time)
        {
            while (CurrentTime < time)
            {
                var nextElement = Elements.MinBy(e => e.NextEventTime);
                var nextEventTime = nextElement.NextEventTime;

                CurrentTime = nextElement.NextEventTime;

                foreach (var element in Elements)
                {
                    element.CurrentTime = CurrentTime;
                }

                nextElement.FinishProcessing();

                foreach (var element in Elements)
                {
                    if (element.NextEventTime == CurrentTime)
                    {
                        element.FinishProcessing();
                    }
                }
            }
        }
    }
}
