namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const int CommandRepair = 0;
        private const int CommandContinue = 1;

        private readonly DetailsStore _detailStore;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;

        internal AutoRepairManager()
        {
            _detailStore = new DetailsStore();
            _cashRegister = new CashRegister();
            _carsQueue = new CarsQueue(_detailStore);
            _warehouse = new Warehouse(_detailStore);
            _garage = new Garage(_warehouse);
        }

        public void SimulateWorkDay()
        {
            int day = 1;

            bool isWork = true;

            while (isWork)
            {

                _carsQueue.AddCars();

                while (_carsQueue.HasCars)
                {
                    Car car = _carsQueue.DequeueCar();

                    RepairReport report;

                    if (Utils.ReadNumber() == CommandRepair)
                    {
                        report = _garage.RepairNextCar(car);
                    }
                    else
                    {
                        report = _garage.RefuseCar();
                    }

                    _cashRegister.Apply(report);
                }

                isWork = Utils.ReadNumber() == CommandContinue;

                day++;
            }
        }
    }
}