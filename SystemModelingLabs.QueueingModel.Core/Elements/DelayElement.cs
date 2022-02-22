using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    public class DelayElement<T> : Element<T> where T : QueueItem
    {
        public override double NextEventTime => itemDelays.Any() ? itemDelays.Min(p => p.Value) : double.MaxValue;
        private Dictionary<T, double> itemDelays = new Dictionary<T, double>();

        public DelayElement(Func<T, double> delayRng) : base(delayRng)
        {
        }

        public override void StartProccesing(T item)
        {
            base.StartProccesing(item);
            itemDelays[item] = CurrentTime + DelayRng(item);
        }

        public override void FinishProcessing()
        {
            base.FinishProcessing();
            var item = itemDelays.MinBy(p => p.Value).Key;
            itemDelays.Remove(item);
            NextElement?.StartProccesing(item);
        }
    }
}
