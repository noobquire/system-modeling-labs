using SystemModelingLabs.QueueingModel.Core;
using SystemModelingLabs.Utils;

namespace SystemModelingLabs.QueueingModel.Bank
{
    public class ClientCarFactory : IItemFactory<ClientCar>
    {
        public ClientCar Create()
        {
            var licensePlate = RandomDataGenerator.GetRandomLicensePlate();
            var clientName = RandomDataGenerator.GetRandomPersonName();

            var car = new ClientCar()
            {
                LicensePlateNumber = licensePlate,
                ClientName = clientName
            };

            return car;
        }
    }
}
