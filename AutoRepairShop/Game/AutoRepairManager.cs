using System;
using System.Collections.Generic;
using AutoRepairShop.UI;

namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private readonly Illustrator _illustrator;
        private readonly PartsFactory _partsFactory;
        private readonly PartsShop _partsShop;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;

        internal AutoRepairManager(Illustrator illustrator)
        {
            if (illustrator == null)
            {
                throw new ArgumentNullException(nameof(illustrator));
            }

            _illustrator = illustrator;
            _partsFactory = new PartsFactory();
            _cashRegister = new CashRegister();
            _carsQueue = new CarsQueue(_partsFactory);
            _warehouse = new Warehouse(_partsFactory);
            _garage = new Garage(_warehouse);
            _partsShop = new PartsShop(_partsFactory, _warehouse, _cashRegister);
        }

        internal void Run()
        {
            _illustrator.ShowBanner(_cashRegister.Money);

            int day = 1;

            bool isWork = true;

            while (isWork)
            {
                _carsQueue.AddCars();

                bool isDayOver = false;

                while (isDayOver == false)
                {
                    DayCommand command = _illustrator.AskDayCommand(day, _cashRegister.Money, _carsQueue.Count);

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
                    }
                }

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

            _cashRegister.Apply(report);

            _illustrator.ShowRepairReport(report, _cashRegister.Money);
            _illustrator.Pause();
        }

        private RepairReport RepairCar(Car car)
        {
            decimal revenue = 0m;

            while (car.CarIsBroken())
            {
                List<Detail> brokenDetails = _garage.DiagnoseFaultyParts(car);
                List<StockItem> stock = BuildStockItems();

                DetailType? type = _illustrator.AskDetailToRepair(car, brokenDetails, stock);

                if (type == null)
                {
                    return _garage.RefuseFurtherRepairs(revenue, car);
                }

                DetailRepairReport report = _garage.RepairDetail(type.Value, car);

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
                List<StockItem> items = BuildStockItems();

                DetailType? type = _illustrator.AskPartToBuy(
                    items,
                    _cashRegister.Money,
                    _warehouse.TotalQuantity,
                    Warehouse.WarehouseCapacity);

                if (type == null)
                {
                    isOpen = false;

                    continue;
                }

                PurchaseReport report = _partsShop.Buy(type.Value);

                _illustrator.ShowPurchase(report, _cashRegister.Money);
                _illustrator.Pause();
            }
        }

        private List<StockItem> BuildStockItems()
        {
            var items = new List<StockItem>();
            var catalog = _partsFactory.GetCatalog();

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                Detail detail;

                if (catalog.TryGetValue(type, out detail))
                {
                    items.Add(new StockItem(detail, _warehouse.GetQuantityDetail(type)));
                }
            }

            return items;
        }
    }
}
