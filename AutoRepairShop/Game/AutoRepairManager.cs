namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const int CommandExit = 0;
        private const int CommandRepair = 1;
        private const int CommandContinue = 2;

        private readonly PartsFactory _partsFactory;
        private readonly PartsShop _partsShop;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;

        internal AutoRepairManager()
        {
            _partsFactory = new PartsFactory();
            _cashRegister = new CashRegister();
            _carsQueue = new CarsQueue(_partsFactory);
            _warehouse = new Warehouse(_partsFactory);
            _garage = new Garage(_warehouse);
            _partsShop = new PartsShop(_partsFactory, _warehouse, _cashRegister);
        }

        internal void SimulateWorkDay()
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
                        report = SuperviseRepairNextCar(car);
                    }
                    else
                    {
                        report = _garage.RefuseRepairs();
                    }

                    _cashRegister.Apply(report);
                }

                isWork = Utils.ReadNumber() == CommandContinue;

                day++;
            }
        }

        private RepairReport SuperviseRepairNextCar(Car car)
        {
            decimal revenueFromRepair = 0m;

            while (car.CarIsBroken())
            {
                int userInput = Utils.ReadNumber();

                if (userInput == CommandExit)
                {
                    return _garage.RefuseFurtherRepairs(revenueFromRepair, car);
                }

                int detailNummber = userInput - 1;

                if (Utils.ContainsDetailType(detailNummber))
                {
                    DetailType type = (DetailType)detailNummber;

                    var report = _garage.RepairDetail(type, car);

                    revenueFromRepair += ReadRepairDetailReport(report);
                }
            }

            return new RepairReport(revenueFromRepair, 0m);
        }

        private decimal ReadRepairDetailReport(DetailRepairReport report)
        {
            decimal result = report.Charge;

            switch (report.Outcome)
            {
                case RepairOutcome.NoDetailInStock:
                    break;

                case RepairOutcome.ReplacedHealthy:
                    break;

                case RepairOutcome.ReplacedBroken:
                    break;
            }

            return result;
        }
    }
}