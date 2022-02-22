using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    public class SwitchableQueueProcessingElement<T> : PriorityQueueProcessingElement<T> where T : QueueItem
    {
        public int QueueSwitchCount { get; private set; } = 0;
        public SwitchableQueueProcessingElement<T>[]? ParallelChannels { get; set; }
        private readonly int queueSwitchThreshold;

        public SwitchableQueueProcessingElement(Func<T, double> delayRng, Func<T, int> priorityFunc, int queueSwitchThreshold = 2, int maxQueueLength = int.MaxValue) : base(delayRng, priorityFunc, maxQueueLength)
        {
            this.queueSwitchThreshold = queueSwitchThreshold;
        }

        public override void FinishProcessing()
        {
            base.FinishProcessing();

            var longerQueueChannel = ParallelChannels?
                .Where(c => c.Queue.Count - this.Queue.Count >= queueSwitchThreshold)
                .FirstOrDefault();
            if(longerQueueChannel != null)
            {
                Console.WriteLine($"[T+{CurrentTime}]: Switching last item from {longerQueueChannel.Name} to {this.Name}");
                longerQueueChannel.QueueSwitchCount++;
                var item = longerQueueChannel.Queue.UnorderedItems.Last();
                
                longerQueueChannel.Queue = new PriorityQueue<T, int>(longerQueueChannel.Queue.UnorderedItems.Take(longerQueueChannel.Queue.Count - 1));
                this.StartProccesing(item.Element);
            }
        }
    }
}
