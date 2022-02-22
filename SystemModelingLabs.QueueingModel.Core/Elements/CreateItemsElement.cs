using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    /// <summary>
    /// Logic element for creating new items for processing.
    /// </summary>
    /// <typeparam name="T">Type of items being produced.</typeparam>
    public class CreateItemsElement<T> : Element<T> where T : QueueItem
    {
        private T nextItem;
        private readonly IItemFactory<T> factory;

        public CreateItemsElement(IItemFactory<T> factory, Func<T, double> delayRng) : base(delayRng)
        {
            this.factory = factory;
            nextItem = factory.Create();
            NextEventTime = CurrentTime + DelayRng(nextItem);
        }

        public override void FinishProcessing()
        {
            base.FinishProcessing();
            nextItem.CreatedAt = CurrentTime;
            NextElement?.StartProccesing(nextItem);
            nextItem = factory.Create();
            NextEventTime = CurrentTime + DelayRng(nextItem);
        }
    }
}
