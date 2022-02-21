using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    public class ParallelProcessingChannelElement<T> : ProcessingChannelElement<T> where T : QueueItem
    {
        public int QueueSwitchCount { get; private set; } = 0;
        public ParallelProcessingChannelElement<T>[]? ParallelChannels { get; set; }
        private readonly int queueSwitchThreshold;

        public ParallelProcessingChannelElement(Func<double> delayRng, int queueSwitchThreshold = 2, int maxQueueLength = int.MaxValue) : base(delayRng, maxQueueLength)
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
                var item = longerQueueChannel.Queue.Last();
                longerQueueChannel.Queue = new Queue<T>(longerQueueChannel.Queue.Take(longerQueueChannel.Queue.Count - 1));
                this.StartProccesing(item);
            }
        }
    }
}
