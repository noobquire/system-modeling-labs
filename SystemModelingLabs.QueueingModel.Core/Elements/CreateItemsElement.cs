using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core.Elements
{
    /// <summary>
    /// Logic element for creating new items for processing.
    /// </summary>
    /// <typeparam name="T">Type of items being produced.</typeparam>
    public class CreateItemsElement<T> : Element<T> where T : QueueItem
    {
        private readonly IItemFactory<T> factory;

        public CreateItemsElement(IItemFactory<T> factory, Func<double> delayRng) : base(delayRng)
        {
            this.factory = factory;
            NextEventTime = DelayRng();
        }

        public override void FinishProcessing()
        {
            base.FinishProcessing();
            NextEventTime = CurrentTime + DelayRng();
            var item = factory.Create();
            NextElement?.StartProccesing(item);
        }
    }
}
