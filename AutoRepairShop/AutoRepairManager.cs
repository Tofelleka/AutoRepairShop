namespace AutoRepairShop
{
    internal class AutoRepairManager
    {

        private readonly DetailsStore _detailStore;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;

        internal AutoRepairManager()
        {
            _detailStore = new DetailsStore();
            _carsQueue = new CarsQueue(_detailStore);
            _warehouse = new Warehouse(_detailStore);
            _garage = new Garage(_warehouse);
        }

        public void SimulateWorkDay()
        {

        }
    }
}