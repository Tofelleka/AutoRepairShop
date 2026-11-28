using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const decimal DailyRent = 1500m;

        private readonly Illustrator _illustrator;
        private readonly PartsFactory _partsFactory;
        private readonly PartsShop _partsShop;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;
        private readonly ServiceDevelopment _serviceDevelopment;
        private readonly UpgradeShop _upgradeShop;

        private readonly List<RepairReport> _dayReports = new List<RepairReport>();

        private decimal _dayPurchaseCosts;
        private decimal _dayUpgradeCosts;

        public AutoRepairManager(Illustrator illustrator)
        {
            if (illustrator == null)
            {
                throw new ArgumentNullException(nameof(illustrator));
            }

            _illustrator = illustrator;

            IRandomSource random = new SystemRandomSource();

            _partsFactory = new PartsFactory(DetailsCatalog.Catalog);
            _cashRegister = new CashRegister();
            _serviceDevelopment = new ServiceDevelopment();

            var carFactory = new CarFactory(DetailsCatalog.Catalog, random);
            var warehouseFactory = new WarehouseFactory(_partsFactory);


            _carsQueue = new CarsQueue(carFactory, random);
            _warehouse = warehouseFactory.Create();
            _upgradeShop = new UpgradeShop(_cashRegister, _warehouse, _serviceDevelopment);
            _garage = new Garage(_warehouse);
            _partsShop = new PartsShop(_partsFactory, _warehouse, _cashRegister);
        }

        public void Run()
        {
            _illustrator.ShowBanner(_cashRegister.Money);

            int day = 1;

            bool isWork = true;

            while (isWork)
            {
                _dayReports.Clear();
                _dayPurchaseCosts = 0m;
                _dayUpgradeCosts = 0m;

                _carsQueue.AddCars();

                bool isDayOver = false;

                while (isDayOver == false)
                {
                    var command = _illustrator.AskDayCommand(day, _cashRegister.Money, _carsQueue.Count);

                    switch (command)
                    {
                        case DayCommand.TakeCar:
                            TakeNextCar();
                            break;

                        case DayCommand.Shop:
                            OpenShop();
                            break;

                        case DayCommand.EndDay:
                            isDayOver = true;
                            break;

                        case DayCommand.Development:
                            OpenDevelopment();
                            break;
                    }
                }

                _cashRegister.Apply(-DailyRent);

                _illustrator.ShowDayStatistics(BuildDayStatistics());
                _illustrator.Pause();

                isWork = _illustrator.AskNextDay(day, _cashRegister.Money);

                day++;
            }

            _illustrator.ShowFinal(_cashRegister.Money, day - 1);
        }

        private void TakeNextCar()
        {
            if (_carsQueue.HasCars == false)
            {
                _illustrator.ShowNoCars();
                _illustrator.Pause();

                return;
            }

            Car car = _carsQueue.DequeueCar();

            RepairReport report = _illustrator.AskRepairCar(car)
                ? RepairCar(car)
                : _garage.RefuseRepairs();

            _cashRegister.Apply(report.Total);
            _dayReports.Add(report);

            _illustrator.ShowRepairReport(report, _cashRegister.Money);
            _illustrator.Pause();
        }

        private RepairReport RepairCar(Car car)
        {
            decimal revenue = 0m;

            while (car.CarIsBroken())
            {
                var brokenDetails = _garage.DiagnoseFaultyParts(car);
                var stock = BuildStockItems();

                DetailType? type = _illustrator.AskDetailToRepair(car, brokenDetails, stock);

                if (type == null)
                {
                    return _garage.RefuseFurtherRepairs(revenue, car);
                }

                var report = _garage.RepairDetail(type.Value, car);

                revenue += report.Charge;

                _illustrator.ShowDetailRepair(report);
                _illustrator.Pause();
            }

            return new RepairReport(revenue, 0m);
        }

        private void OpenShop()
        {
            bool isOpen = true;

            while (isOpen)
            {
                var items = BuildStockItems();

                DetailType? type = _illustrator.AskPartToBuy(
                    items,
                    _cashRegister.Money,
                    _warehouse.TotalQuantity,
                    _warehouse.Capacity);

                if (type == null)
                {
                    isOpen = false;

                    continue;
                }

                var report = _partsShop.Buy(type.Value);

                if (report.Outcome == PurchaseOutcome.Bought)
                {
                    _dayPurchaseCosts += report.Detail.Price;
                }

                _illustrator.ShowPurchase(report, _cashRegister.Money);
                _illustrator.Pause();
            }
        }

        private void OpenDevelopment()
        {
            bool isOpen = true;

            while (isOpen)
            {
                if (_illustrator.AskWarehouseUpgrade(_warehouse, _development) == false)
                {
                    isOpen = false;

                    continue;
                }

                UpgradeReport report = _upgradeShop.BuyWarehouseUpgrade();

                if (report.Outcome == UpgradeOutcome.Bought)
                {
                    _dayUpgradeCosts += report.Price;
                }

                _illustrator.ShowUpgradeResult(report, _cashRegister.Money, _warehouse.Capacity);
                _illustrator.Pause();
            }
        }

        private List<StockItem> BuildStockItems()
        {
            return _partsFactory.GetAll()
                .Select(pair => new StockItem(pair.Value, _warehouse.GetQuantityDetail(pair.Key)))
                .ToList();
        }

        private DayStatistics BuildDayStatistics()
        {
            int cars = _dayReports.Count;
            int repaired = _dayReports.Count(report => report.Fine == 0m);
            int refused = _dayReports.Count(report => report.Fine > 0m);
            decimal fines = _dayReports.Sum(report => report.Fine);
            decimal revenue = _dayReports.Sum(report => report.Revenue);

            return new DayStatistics(cars, refused, repaired, revenue, fines, DailyRent, _dayPurchaseCosts, _dayUpgradeCosts);
        }
    }
}
