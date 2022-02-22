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
    public class PriorityQueueProcessingElement<T> : Element<T> where T : QueueItem
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
        public PriorityQueue<T, int> Queue { get; set; } = new PriorityQueue<T, int>();
        protected readonly Func<T, int> priorityFunc;

        public PriorityQueueProcessingElement(Func<T, double> delayRng, Func<T, int> priorityFunc, int maxQueueLength = int.MaxValue, int channels = 1) : base(delayRng)
        {
            MaxQueueLength = maxQueueLength;
            CurrentItems = new T[channels];
            EventTimes = new double[channels];
            for(int i = 0; i < channels; i++)
            {
                EventTimes[i] = double.MaxValue;
            }
            ChannelStates = new ChannelState[channels];
            this.priorityFunc = priorityFunc;
        }

        public T?[] CurrentItems { get; init; }

        public ChannelState[] ChannelStates { get; init; }

        public double[] EventTimes { get; init; }

        public override double NextEventTime => EventTimes.Min();

        public override void StartProccesing(T item)
        {
            if (ChannelStates.Any(c => c == ChannelState.Free))
            {
                base.StartProccesing(item);
                int freeChannelId = Array.FindIndex(ChannelStates, c => c == ChannelState.Free);
                CurrentItems[freeChannelId] = item;
                ChannelStates[freeChannelId] = ChannelState.Processing;
                var deltaTime = DelayRng(item);
                EventTimes[freeChannelId] = CurrentTime + deltaTime;
                TotalProcessingTime += deltaTime;
            }
            else if (Queue.Count < MaxQueueLength)
            {
                Queue.Enqueue(item, priorityFunc(item));
                TotalQueueLengthTime += (NextEventTime - CurrentTime) * Queue.Count;
            }
            else
            {
                FailureCount++;
                return;
            }
        }

        public override void FinishProcessing()
        {
            Console.WriteLine($"[T+{CurrentTime}]: {Name} finished processing item");
            var finishedItemsIndexes = EventTimes
                .Select((e, i) => (e, i))
                .Where(p => p.e == CurrentTime);
                
            ProcessedItemsCount += finishedItemsIndexes.Count();
            TotalTimeBetweenProcessing += CurrentTime - LastProcessingFinishedTime;
            LastProcessingFinishedTime = CurrentTime;

            // remove processed items
            var finishedItems = new List<T>();
            foreach(var pair in finishedItemsIndexes)
            {
                ChannelStates[pair.i] = ChannelState.Free;
                finishedItems.Add(CurrentItems[pair.i]);
                CurrentItems[pair.i] = null;
                EventTimes[pair.i] = double.MaxValue;
            }
            
            // fill free slots
            
            while(Queue.Count > 0 && ChannelStates.Any(c => c == ChannelState.Free))
            {
                StartProccesing(Queue.Dequeue());
                /*
                int freeSlotId = Array.FindIndex(ChannelStates, c => c == ChannelState.Free);
                
                CurrentItems[freeSlotId] = Queue.Dequeue();
                ChannelStates[freeSlotId] = ChannelState.Processing;
                EventTimes[freeSlotId] = CurrentTime + DelayRng();
                */
            }

            foreach(var item in finishedItems)
            {
                item.ExitedAt = CurrentTime;
                NextElement?.StartProccesing(item);
            }
        }
    }
}
