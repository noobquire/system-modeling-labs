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

        protected readonly Func<T, double> DelayRng;

        protected Element(Func<T, double> delayRng)
        {
            DelayRng = delayRng;
        }

        public Element<T>? NextElement;

        public virtual double CurrentTime { get; set; } = 0;
        public virtual double NextEventTime { get; set; } = double.MaxValue;
        public int ProcessedItemsCount { get; protected set; }

        public virtual void StartProccesing(T item)
        {
            Console.WriteLine($"[T+{CurrentTime}]: {Name} started processing item {item}");
        }

        public virtual void FinishProcessing()
        {
            Console.WriteLine($"[T+{CurrentTime}]: {Name} finished processing item");
            ProcessedItemsCount++;
        }
    }
}
