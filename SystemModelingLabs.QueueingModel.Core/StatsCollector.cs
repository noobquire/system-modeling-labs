using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemModelingLabs.QueueingModel.Core.Elements;
using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Core
{
    /// <summary>
    /// Collects statistics for simulated queueing model.
    /// </summary>
    public class StatsCollector<T> where T : QueueItem 
    {
        private readonly QueueingModel<T> model;

        public StatsCollector(QueueingModel<T> model)
        {
            this.model = model;
        }

        private Dictionary<string, double> GetAverageProcessorBusiness()
        {
            return model.Elements.Where(e => e is ProcessingChannelElement<T>)
                .ToDictionary(e => e.Name, e =>
                {
                    var processor = (ProcessingChannelElement<T>)e;
                    return processor.TotalProcessingTime / processor.ProcessedItemsCount;
                });
        }

        private double GetAverageItemsCount(double time)
        {
            return model.TotalItemsCreated / time;
        }

        private double GetAverageProcessingFinishedInterval()
        {
            return ProcessingChannelElement<T>.TotalTimeBetweenProcessing / model.TotalItemsProcessed;
        }

        private double GetAverageProcessingTime(double time)
        {
            return time / model.TotalItemsCreated;
        }

        private Dictionary<string, double> GetAverageQueueLength()
        {
            return model.Elements.Where(e => e is ProcessingChannelElement<T>)
                .ToDictionary(e => e.Name, e =>
                {
                    var processor = (ProcessingChannelElement<T>)e;
                    return processor.TotalQueueLengthTime / model.CurrentTime;
                });
        }

        private double GetFailurePercentage()
        {
            return model.TotalFailures / (double)model.TotalItemsCreated;
        }

        private SimulationStatistics GetStats()
        {
            return new SimulationStatistics
            {
                AverageProcessorBusiness = GetAverageProcessorBusiness(),
                AverageItemsCount = GetAverageItemsCount(model.CurrentTime),
                AverageProcessingFinishedInterval = GetAverageProcessingFinishedInterval(),
                AverageProcessingTime = GetAverageProcessingTime(model.CurrentTime),
                AverageQueueLength = GetAverageQueueLength(),
                FailurePercentage = GetFailurePercentage(),
                QueueChangeAmount = model.TotalQueueChanges
            };
        }

        public void PrintStatistics()
        {
            var stats = GetStats();
            Console.WriteLine(stats.ToString());
            Console.WriteLine($"Total items created: {model.TotalItemsCreated}");
            Console.WriteLine($"Total items processed: {model.TotalItemsProcessed}");
        }
    }
}
