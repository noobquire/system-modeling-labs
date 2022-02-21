using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Bank
{
    public class ClientCar : QueueItem
    {
        public string LicensePlateNumber { get; set; }

        public string ClientName { get; set; }
    }
}
