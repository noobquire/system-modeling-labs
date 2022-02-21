using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{

    /// <summary>
    /// Base class for logic elements composing a queueing model.
    /// </summary>
    /// <typeparam name="T">Type of items being processed.</typeparam>
    public abstract class Element<T> where T : QueueItem
    {
        public string Name { get; init; }

        protected readonly Func<double> DelayRng;

        protected Element(Func<double> delayRng)
        {
            DelayRng = delayRng;
        }

        public Element<T>? NextElement;

        public double CurrentTime { get; set; } = 0;
        public double NextEventTime { get; set; } = double.MaxValue;
        public int ProcessedItemsCount { get; private set; }

        public virtual void StartProccesing(T item)
        {
            Console.WriteLine($"[T+{CurrentTime}]: {Name} started processing item");
        }

        public virtual void FinishProcessing()
        {
            Console.WriteLine($"[T+{CurrentTime}]: {Name} finished processing item");
            ProcessedItemsCount++;
        }
    }
}
