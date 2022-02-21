using System.Text;

namespace SystemModelingLabs.QueueingModel.Core.Models
{
    public class SimulationStatistics
    {
        /// <summary>
        /// Average time each processing element was busy processing items.
        /// </summary>
        public Dictionary<string, double> AverageProcessorBusiness { get; set; }

        /// <summary>
        /// Average amount of items in system.
        /// </summary>
        public double AverageItemsCount { get; set; }

        /// <summary>
        /// Average time interval between finishing processing items.
        /// </summary>
        public double AverageProcessingFinishedInterval { get; set; }

        /// <summary>
        /// Average time spent item spends in system.
        /// </summary>
        public double AverageProcessingTime { get; set; }

        /// <summary>
        /// Average queue length per each processor.
        /// </summary>
        public Dictionary<string, double> AverageQueueLength { get; set; }

        /// <summary>
        /// Percentage of failed to process items.
        /// </summary>
        public double FailurePercentage { get; set; }

        /// <summary>
        /// How many times items were transferred to queue in another processor.
        /// </summary>
        public double QueueChangeAmount { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Average processing business per processor:");
            foreach(var item in AverageProcessorBusiness)
            {
                sb.AppendLine($"{item.Key} - {item.Value:P}");
            }
            sb.AppendLine($"Average items count in system: {AverageItemsCount}");
            sb.AppendLine($"Average time between finishing processing: {AverageProcessingFinishedInterval}");
            sb.AppendLine($"Average item processing time: {AverageProcessingTime}");
            sb.AppendLine($"Avegage queue length per processor:");
            foreach (var item in AverageQueueLength)
            {
                sb.AppendLine($"{item.Key} - {item.Value}");
            }
            sb.AppendLine($"Failure percentage: {FailurePercentage:P}");
            sb.AppendLine($"Queue changes: {QueueChangeAmount}");
            return sb.ToString();
        }
    }
}
