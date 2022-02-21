using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    public enum ChannelState
    {
        Free,
        Processing
    }

    /// <summary>
    /// Logic element for processing items.
    /// </summary>
    /// <typeparam name="T">Type of items being processed.</typeparam>
    public class ProcessingChannelElement<T> : Element<T> where T : QueueItem
    {
        public double TotalProcessingTime { get; private set; } = 0;
        public int FailureCount { get; private set; } = 0;
        public int MaxQueueLength { get; set; }
        public static double LastProcessingFinishedTime { get; private set; }
        public static double TotalTimeBetweenProcessing { get; private set; }
        /// <summary>
        /// Sum of queue length for each time between processing
        /// </summary>
        public int TotalQueueLength { get; set; }
        public double TotalQueueLengthTime { get; set; }
        public Queue<T> Queue { get; set; } = new Queue<T>();

        public ProcessingChannelElement(Func<double> delayRng, int maxQueueLength = int.MaxValue) : base(delayRng)
        {
            MaxQueueLength = maxQueueLength;
        }

        public T? CurrentItem { get; set; }

        public ChannelState State { get; private set; } = ChannelState.Free;

        public override void StartProccesing(T item)
        {
            base.StartProccesing(item);
            if (State == ChannelState.Free)
            {
                CurrentItem = item;
                State = ChannelState.Processing;
                var deltaTime = DelayRng();
                NextEventTime = CurrentTime + deltaTime;
                TotalProcessingTime += deltaTime;
            }
            else if (Queue.Count < MaxQueueLength)
            {
                Queue.Enqueue(item);
            }
            else
            {
                FailureCount++;
                return;
            }
            TotalQueueLengthTime += (NextEventTime - CurrentTime) * Queue.Count;
        }

        public override void FinishProcessing()
        {
            base.FinishProcessing();
            var item = CurrentItem;
            TotalTimeBetweenProcessing += CurrentTime - LastProcessingFinishedTime;
            LastProcessingFinishedTime = CurrentTime;
            State = ChannelState.Free;
            CurrentItem = null;

            NextEventTime = double.MaxValue;
            if (Queue.Any())
            {
                CurrentItem = Queue.Dequeue();
                State = ChannelState.Processing;
                NextEventTime = CurrentTime + DelayRng();
            }

            NextElement?.StartProccesing(item);
        }
    }
}
