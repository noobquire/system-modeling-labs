using SystemModelingLabs;
using SystemModelingLabs.QueueingModel.Bank;
using SystemModelingLabs.QueueingModel.Core;
using SystemModelingLabs.QueueingModel.Core.Elements;
using SystemModelingLabs.QueueingModel.Core.Models;
using SystemModelingLabs.Utils;

var factory = new ClientCarFactory();
var creationRate = () => RandomUtils.NextExponential(2);
var createElement = new CreateItemsElement<ClientCar>(factory, creationRate) { Name = "customer cars entry" };

var processingRate = () => RandomUtils.NextExponential(3);
var cashierProcessing1 = new ParallelProcessingChannelElement<ClientCar>(processingRate, maxQueueLength: 3) { Name = "first cashier" };
var cashierProcessing2 = new ParallelProcessingChannelElement<ClientCar>(processingRate, maxQueueLength: 3) { Name = "second cashier" };

cashierProcessing1.ParallelChannels = new[] { cashierProcessing2 };
cashierProcessing2.ParallelChannels = new[] { cashierProcessing1 };

DecisionFunction<ClientCar> decisionFunc = (cashiers, item) =>
    cashiers.MinBy(e => ((ProcessingChannelElement<ClientCar>)e).Queue.Count);
var cashiers = new[] { cashierProcessing1, cashierProcessing2 };
var queueDecisionElement = new ConditionElement<ClientCar>(cashiers, decisionFunc) { Name = "cashier queue decision"};

createElement.NextElement = queueDecisionElement;

var elements = new Element<ClientCar>[]
{
    createElement,
    queueDecisionElement,
    cashierProcessing1,
    cashierProcessing2
};

var model = new QueueingModel<ClientCar>(elements);

model.Simulate(3000);
Console.WriteLine();
var stats = new StatsCollector<ClientCar>(model);
stats.PrintStatistics();