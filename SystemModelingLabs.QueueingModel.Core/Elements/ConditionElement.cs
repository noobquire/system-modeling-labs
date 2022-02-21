using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    /// <summary>
    /// Zero-delay condition to choose the next element for processing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConditionElement<T> : Element<T> where T : QueueItem
    {
        private T? currentItem { get; set; }
        private readonly IEnumerable<Element<T>> outputs;
        DecisionFunction<T> decision;

        public ConditionElement(IEnumerable<Element<T>> outputs,
            DecisionFunction<T> decision) 
            : base(() => double.MaxValue)
        {
            this.outputs = outputs;
            this.decision = decision;
        }

        public override void StartProccesing(T item)
        {
            base.StartProccesing(item);
            NextEventTime = CurrentTime;
            currentItem = item;
        }

        public override void FinishProcessing()
        {
            if(currentItem == null)
            {
                return;
            }

            base.FinishProcessing();

            var output = decision(outputs, currentItem);

            output.StartProccesing(currentItem);
            currentItem = null;
            NextEventTime = double.MaxValue;
        }
    }
}
