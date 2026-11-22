namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const int CommandRepair = 0;
        private const int CommandContinue = 1;
        private const int CommandExit = 0;

        private readonly DetailsStore _detailStore;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;

        internal AutoRepairManager()
        {
            _detailStore = new DetailsStore();
            _carsQueue = new CarsQueue(_detailStore);
            _warehouse = new Warehouse(_detailStore);
            _garage = new Garage(_warehouse);
            _cashRegister = new CashRegister(_garage);
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

                    int income;

                    if (Utils.ReadNumber() == CommandRepair)
                    {
                        income = _garage.RepairNextCar(car);
                    }
                    else
                    {
                        income = _garage.RefuseCar();
                    }

                    _cashRegister.CalculateProfitFromRepair(income);
                }

                isWork = Utils.ReadNumber() == CommandContinue;

                day++;
            }
        }
    }
}