namespace SystemModelingLabs.QueueingModel.Core.Models
{
    /// <summary>
    /// Base class for items being processed by the system
    /// </summary>
    public abstract class QueueItem
    {
        public int Id { get; set; }

        public double CreatedAt { get; set; }

        public double ExitedAt { get; set; }
    }
}
