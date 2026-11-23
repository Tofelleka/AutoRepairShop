using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const int CommandRepair = 0;
        private const int CommandContinue = 1;

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
            var brokenDetails = _garage.DiagnoseFaultyParts(car);

            decimal revenueFromRepair = 0m;

            while (car.CarIsBroken())
            {
                int userInput = Utils.ReadNumber();

                if (Utils.ContainsDetailType(userInput))
                {
                    DetailType type = (DetailType)userInput;

                    if(type == DetailType.Exit)
                    {
                        return _garage.RefuseFurtherRepairs(revenueFromRepair, car);
                    }

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

        private void MakePurchase()
        {
            DetailType type = DetailType.Generator;

            PurchaseReport report = _partsShop.Buy(type);

            switch(report.Outcome)
            {
                case PurchaseOutcome.Bought:
                    break;

                case PurchaseOutcome.NotEnoughMoney:
                    break;

                case PurchaseOutcome.WarehouseIsFull:
                    break;

                default:
                    break;
            }
        }
    }
}