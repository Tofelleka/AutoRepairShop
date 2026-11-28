using System;
using System.Collections.Generic;
using System.Linq;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal class SeededRandom : IRandomSource
    {
        private readonly Random _random;

        public SeededRandom(int seed)
        {
            _random = new Random(seed);
        }

        public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);

        public int NextInclusive(int minValue, int maxValue) => _random.Next(minValue, maxValue + 1);

        public bool NextChance(double chance) => _random.NextDouble() < chance;
    }

    internal class BalanceSimulator
    {
        private const decimal Reserve = 50_000m;
        private const decimal RefuseFine = 1_000m;
        private const decimal DailyRent = GameBalance.DailyRent;
        private const decimal GoalMoney = GameBalance.GoalMoney;

        private readonly IRandomSource _random;
        private readonly CarFactory _carFactory;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;
        private readonly ServiceDevelopment _development;
        private readonly PartsShop _shop;
        private readonly UpgradeShop _upgradeShop;
        private readonly Education _education;
        private readonly double _sloppiness;

        private int _bodyBoxBusyDay;
        private decimal _totalEarned;

        public BalanceSimulator(int seed, double sloppiness = 0d)
        {
            _random = new SeededRandom(seed);
            _carFactory = new CarFactory(_random);

            var partsFactory = new PartsFactory();
            var warehouseFactory = new WarehouseFactory(partsFactory);

            _warehouse = warehouseFactory.Create();
            _cashRegister = new CashRegister();
            _development = new ServiceDevelopment();
            _garage = new Garage(_warehouse, _random);
            _shop = new PartsShop(_warehouse, _cashRegister);
            _upgradeShop = new UpgradeShop(_cashRegister, _development);
            _education = new Education();
            _sloppiness = sloppiness;
        }

        public decimal Money => _cashRegister.Money;

        public decimal Earned => _totalEarned;

        public bool IsBankrupt { get; private set; }

        public bool HasCourse(CourseType type) => _education.IsPassed(type);

        public int Run(int maxDays)
        {
            for (int day = 1; day <= maxDays; day++)
            {
                int cars = 1 + _random.NextInclusive(0, GetMaxCarsPerDay());

                for (int i = 0; i < cars; i++)
                {
                    HandleCar(day);
                }

                _cashRegister.Apply(-DailyRent);

                if (_cashRegister.Money < 0m)
                {
                    IsBankrupt = true;

                    return -1;
                }

                BuyUpgrades();
                BuyCourses();

                if (_cashRegister.Money >= GoalMoney)
                {
                    return day;
                }
            }

            return -1;
        }

        private int GetMaxCarsPerDay() =>
            CarsQueue.DefaultMaxCarsPerDay + UpgradeCatalog.Get(UpgradeType.Marketing).EffectPerLevel
                * _development.GetLevel(UpgradeType.Marketing);

        private void HandleCar(int day)
        {
            EnsureWarehouseRoom();

            if (_random.NextChance(_sloppiness * 0.25d))
            {
                _cashRegister.Apply(-RefuseFine);

                return;
            }

            if (TryRepaint(day))
            {
                return;
            }

            Car car = _carFactory.Create(PickBrand());

            foreach (Detail broken in car.GetAllDetails().Where(detail => detail.IsBroken).ToList())
            {
                if (CanRepair(broken.Category) == false)
                {
                    continue;
                }

                if (_random.NextChance(_sloppiness * 0.15d))
                {
                    continue;
                }

                EnsureStock(broken.Type);

                if (_warehouse.GetQuantityDetail(broken.Type) == 0)
                {
                    continue;
                }

                DetailRepairReport report = _garage.RepairDetail(broken.Type, car, GetWorkRate());

                _cashRegister.Apply(report.Charge);
                _totalEarned += report.Charge;
            }

            if (car.CarIsBroken())
            {
                _cashRegister.Apply(-RefuseFine);
            }
        }

        private CarBrand PickBrand()
        {
            var brands = new List<CarBrand> { CarBrand.Vaz };

            foreach (CourseInfo info in CourseCatalog.All)
            {
                if (_education.IsPassed(info.Type))
                {
                    brands.Add(info.Brand);
                }
            }

            return brands[_random.Next(0, brands.Count)];
        }

        private bool TryRepaint(int day)
        {
            if (_development.GetLevel(UpgradeType.BodyBox) == 0)
            {
                return false;
            }

            if (day < _bodyBoxBusyDay)
            {
                return false;
            }

            if (_random.NextChance(GameBalance.RepaintChance) == false)
            {
                return false;
            }

            EnsureStock(DetailType.PaintWhite);
            EnsureStock(DetailType.Primer);
            EnsureStock(DetailType.Lacquer);

            if (_warehouse.GetQuantityDetail(DetailType.PaintWhite) == 0
                || _warehouse.GetQuantityDetail(DetailType.Primer) == 0
                || _warehouse.GetQuantityDetail(DetailType.Lacquer) == 0)
            {
                return false;
            }

            _warehouse.TryTake(DetailType.PaintWhite, out Detail _);
            _warehouse.TryTake(DetailType.Primer, out Detail _);
            _warehouse.TryTake(DetailType.Lacquer, out Detail _);

            _cashRegister.Apply(GameBalance.RepaintPrice);
            _totalEarned += GameBalance.RepaintPrice;
            _bodyBoxBusyDay = day + GameBalance.RepaintDays;

            return true;
        }

        private bool CanRepair(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Body:
                    return _development.GetLevel(UpgradeType.BodyBox) > 0;

                case PartCategory.Electric:
                    return _development.GetLevel(UpgradeType.ElectricBox) > 0;

                case PartCategory.Material:
                    return false;

                default:
                    return true;
            }
        }

        private void EnsureWarehouseRoom()
        {
            if (_warehouse.TotalQuantity < _warehouse.Capacity - 10)
            {
                return;
            }

            UpgradeInfo info = UpgradeCatalog.Get(UpgradeType.Warehouse);

            if (_development.CanUpgrade(UpgradeType.Warehouse) == false)
            {
                return;
            }

            decimal price = info.GetPrice(_development.GetLevel(UpgradeType.Warehouse));

            if (_cashRegister.CanAfford(price + Reserve) == false)
            {
                return;
            }

            if (_upgradeShop.Buy(UpgradeType.Warehouse).Outcome == UpgradeOutcome.Bought)
            {
                _warehouse.IncreaseCapacity(info.EffectPerLevel);
            }
        }

        private void EnsureStock(DetailType type)
        {
            if (_warehouse.GetQuantityDetail(type) > 0)
            {
                return;
            }

            PartTemplate template = DetailsCatalog.GetTemplate(type);
            decimal price = template.GetPrice(DetailsCatalog.StandardBrand);

            if (_cashRegister.CanAfford(price))
            {
                _shop.Buy(type, DetailsCatalog.StandardBrand);
            }
        }

        private decimal GetWorkRate() =>
            1m + 0.01m * UpgradeCatalog.Get(UpgradeType.Skill).EffectPerLevel
                * _development.GetLevel(UpgradeType.Skill);

        private void BuyUpgrades()
        {
            if (_random.NextChance(_sloppiness * 0.2d))
            {
                return;
            }

            TryBuy(UpgradeType.BodyBox);
            TryBuy(UpgradeType.ElectricBox);
            TryBuy(UpgradeType.Skill);
            TryBuy(UpgradeType.Marketing);
            TryBuy(UpgradeType.Warehouse);
        }

        private void TryBuy(UpgradeType type)
        {
            if (_development.CanUpgrade(type) == false)
            {
                return;
            }

            UpgradeInfo info = UpgradeCatalog.Get(type);
            decimal price = info.GetPrice(_development.GetLevel(type));

            if (_cashRegister.CanAfford(price + Reserve) == false)
            {
                return;
            }

            UpgradeReport report = _upgradeShop.Buy(type);

            if (report.Outcome == UpgradeOutcome.Bought && type == UpgradeType.Warehouse)
            {
                _warehouse.IncreaseCapacity(info.EffectPerLevel);
            }
        }

        private void BuyCourses()
        {
            foreach (CourseInfo info in CourseCatalog.All)
            {
                if (_education.IsPassed(info.Type))
                {
                    continue;
                }

                if (_totalEarned < info.RequiredEarned)
                {
                    continue;
                }

                if (_cashRegister.CanAfford(info.Price + Reserve) == false)
                {
                    continue;
                }

                _cashRegister.Pay(info.Price);
                _education.Pass(info.Type);

                return;
            }
        }
    }
}
