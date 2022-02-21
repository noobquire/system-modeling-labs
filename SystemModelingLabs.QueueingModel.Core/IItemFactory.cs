using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core
{
    /// <summary>
    /// Factory interface for creating new item instances.
    /// </summary>
    /// <typeparam name="T">Type of item instances.</typeparam>
    public interface IItemFactory<T> where T : QueueItem
    {
        /// <summary>
        /// Create new item instance.
        /// </summary>
        /// <returns>New item instance.</returns>
        T Create();
    }
}
