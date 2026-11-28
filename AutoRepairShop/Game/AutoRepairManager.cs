using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private readonly Illustrator _illustrator;
        private readonly PartsShop _partsShop;
        private readonly CarsQueue _carsQueue;
        private readonly Warehouse _warehouse;
        private readonly Garage _garage;
        private readonly CashRegister _cashRegister;
        private readonly ServiceDevelopment _serviceDevelopment;
        private readonly UpgradeShop _upgradeShop;
        private readonly GameGoal _goal;
        private readonly IRandomSource _random;
        private readonly Profile _profile;
        private readonly Education _education;
        private readonly string _savePath;
        private readonly bool _isSaveLoaded;
        private readonly ShiftStatistics _savedShift;

        private readonly List<RepairReport> _dayReports = new List<RepairReport>();
        private readonly List<DayStatistics> _dayStatistics = new List<DayStatistics>();

        private decimal _dayPurchaseCosts;
        private decimal _dayUpgradeCosts;
        private decimal _totalEarned;
        private int _bodyBoxBusyUntilDay;
        private int _day;

        public AutoRepairManager(Illustrator illustrator, int saveSlot, bool continueSavedGame)
        {
            if (illustrator == null)
            {
                throw new ArgumentNullException(nameof(illustrator));
            }

            _illustrator = illustrator;
            _savePath = GameSave.GetPath(saveSlot);
            _goal = new GameGoal(GameBalance.GoalTitle, GameBalance.GoalMoney);
            _profile = Profile.Load();

            _random = new SystemRandomSource();

            GameSave save = continueSavedGame ? LoadSave() : null;

            _isSaveLoaded = save != null;
            _savedShift = save == null ? GameSave.CreateEmptyShift() : save.Shift;
            _bodyBoxBusyUntilDay = save == null ? 0 : save.BodyBusyUntilDay;
            _totalEarned = save == null ? 0m : save.Earned;
            _education = save == null
                ? new Education()
                : new Education(save.Courses);

            var partsFactory = new PartsFactory();
            var carFactory = new CarFactory(_random);
            var warehouseFactory = new WarehouseFactory(partsFactory);

            if (save == null)
            {
                _cashRegister = new CashRegister();
                _serviceDevelopment = new ServiceDevelopment();
                _day = 1;
            }
            else
            {
                _cashRegister = new CashRegister(save.Money);
                _serviceDevelopment = new ServiceDevelopment(save.Levels);
                _day = save.Day;
            }

            int capacity = Warehouse.DefaultCapacity
                + UpgradeCatalog.Get(UpgradeType.Warehouse).EffectPerLevel * _serviceDevelopment.GetLevel(UpgradeType.Warehouse);

            _carsQueue = new CarsQueue(carFactory, _random);
            _warehouse = save == null ? warehouseFactory.Create() : warehouseFactory.Create(capacity, save.Stock);
            _upgradeShop = new UpgradeShop(_cashRegister, _serviceDevelopment);
            _garage = new Garage(_warehouse, _random);
            _partsShop = new PartsShop(_warehouse, _cashRegister);

            if (save == null)
            {
                SaveGame(1);
            }
        }

        public void Run()
        {
            _illustrator.ShowBanner(_cashRegister.Money, _goal, _day, _isSaveLoaded);

            GameEndReason? endReason = null;

            while (endReason == null)
            {
                _dayReports.Clear();
                _dayPurchaseCosts = 0m;
                _dayUpgradeCosts = 0m;

                _carsQueue.AddCars(GetMaxCarsPerDay(), BuildAvailableBrands());

                bool isDayOver = false;

                while (isDayOver == false)
                {
                    DayCommand command = _illustrator.AskDayCommand(
                        _day,
                        _cashRegister.Money,
                        _carsQueue.Count,
                        _goal,
                        GetBoxStatus());

                    switch (command)
                    {
                        case DayCommand.TakeCar:
                            TakeNextCar();
                            break;

                        case DayCommand.Shop:
                            OpenShop();
                            break;

                        case DayCommand.Development:
                            OpenDevelopment();
                            break;

                        case DayCommand.Achievements:
                            _illustrator.ShowAchievements(_profile);
                            break;

                        case DayCommand.Courses:
                            OpenCourses();
                            break;

                        case DayCommand.EndDay:
                            isDayOver = true;
                            break;
                    }
                }

                _cashRegister.Apply(-GameBalance.DailyRent);

                DayStatistics statistics = BuildDayStatistics();
                _dayStatistics.Add(statistics);

                _illustrator.ShowDayStatistics(statistics);
                _illustrator.Pause();

                if (_cashRegister.Money >= 1_000_000m)
                {
                    Unlock(AchievementType.Millionaire);
                }

                SaveGame(_day + 1);

                endReason = EvaluateDayEnd();

                if (endReason == null)
                {
                    if (_illustrator.AskNextDay(_day, _cashRegister.Money) == false)
                    {
                        endReason = GameEndReason.StoppedByPlayer;
                    }
                    else
                    {
                        _day++;
                    }
                }
            }

            if (endReason == GameEndReason.StoppedByPlayer)
            {
                SaveGame(_day + 1);
            }
            else
            {
                DeleteSave();
            }

            if (_profile.BeatsRecord(_cashRegister.Money))
            {
                _profile.UpdateRecord(_cashRegister.Money, _day);
            }

            _profile.Save();

            _illustrator.ShowFinal(_cashRegister.Money, BuildShiftStatistics(), endReason.Value, _profile);
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

            if (TryTakeRepaintOrder(car))
            {
                return;
            }

            RepairReport report = _illustrator.AskRepairCar(car)
                ? RepairCar(car)
                : _garage.RefuseRepairs();

            _cashRegister.Apply(report.Total);
            _totalEarned += report.Revenue;
            _dayReports.Add(report);

            _illustrator.ShowRepairReport(report, _cashRegister.Money);
            _illustrator.Pause();
        }

        private bool TryTakeRepaintOrder(Car car)
        {
            if (_serviceDevelopment.GetLevel(UpgradeType.BodyBox) == 0)
            {
                return false;
            }

            if (_day < _bodyBoxBusyUntilDay)
            {
                return false;
            }

            if (_random.NextChance(GameBalance.RepaintChance) == false)
            {
                return false;
            }

            PaintColor orderedColor = PaintColors.All[_random.Next(0, PaintColors.All.Count)];

            if (HasRepaintMaterials(orderedColor) == false)
            {
                return false;
            }

            if (_illustrator.AskRepaint(car, GameBalance.RepaintPrice, GameBalance.RepaintDays, orderedColor) == false)
            {
                return true;
            }

            PaintColor? usedColor = _illustrator.AskPaintColor(BuildPaintQuantities());

            if (usedColor == null)
            {
                return true;
            }

            ConsumeMaterial(PaintColors.ToDetailType(usedColor.Value));
            ConsumeMaterial(DetailType.Primer);
            ConsumeMaterial(DetailType.Lacquer);

            _bodyBoxBusyUntilDay = _day + GameBalance.RepaintDays;

            if (usedColor.Value == orderedColor)
            {
                _cashRegister.Apply(GameBalance.RepaintPrice);
                _totalEarned += GameBalance.RepaintPrice;
                _dayReports.Add(new RepairReport(GameBalance.RepaintPrice, 0m));

                _profile.IncrementRepaints();
                _profile.Save();

                Unlock(AchievementType.FirstRepaint);

                if (_profile.SuccessfulRepaints >= 3)
                {
                    Unlock(AchievementType.RepaintMaster);
                }

                _illustrator.ShowRepaintDone(GameBalance.RepaintPrice, _bodyBoxBusyUntilDay);
            }
            else
            {
                Unlock(AchievementType.Painter);

                _illustrator.ShowRepaintFailed(orderedColor, usedColor.Value);
            }

            _illustrator.Pause();

            return true;
        }

        private bool HasRepaintMaterials(PaintColor color) =>
            _warehouse.GetQuantityDetail(PaintColors.ToDetailType(color)) > 0
            && _warehouse.GetQuantityDetail(DetailType.Primer) > 0
            && _warehouse.GetQuantityDetail(DetailType.Lacquer) > 0;

        private IReadOnlyDictionary<DetailType, int> BuildPaintQuantities()
        {
            var paints = new Dictionary<DetailType, int>();

            foreach (PaintColor color in PaintColors.All)
            {
                DetailType type = PaintColors.ToDetailType(color);
                paints[type] = _warehouse.GetQuantityDetail(type);
            }

            return paints;
        }

        private void ConsumeMaterial(DetailType type)
        {
            if (_warehouse.TryTake(type, out Detail _) == false)
            {
                throw new InvalidOperationException($"Материал {type} закончился на складе.");
            }
        }

        private RepairReport RepairCar(Car car)
        {
            decimal revenue = 0m;
            WorkRates rates = BuildWorkRates();
            ServiceAccess access = BuildServiceAccess();

            while (car.CarIsBroken())
            {
                List<Detail> brokenDetails = _garage.DiagnoseFaultyParts(car);

                RepairAction action = _illustrator.AskRepairAction(car, brokenDetails, rates, access);

                if (action == RepairAction.Cancel)
                {
                    return _garage.RefuseFurtherRepairs(revenue, car);
                }

                if (action == RepairAction.Repair)
                {
                    List<Detail> repairable = brokenDetails.FindAll(
                        detail => detail.IsRepairable && access.CanRepair(detail.Category));

                    DetailType? repairType = _illustrator.AskRepairableDetail(car, repairable, rates);

                    if (repairType == null)
                    {
                        continue;
                    }

                    PartCategory repairCategory = DetailsCatalog.GetCategory(repairType.Value);

                    DetailRepairReport repairReport = _garage.RepairWithoutReplacement(
                        repairType.Value,
                        car,
                        rates.GetRate(repairCategory));

                    revenue += repairReport.Charge;

                    _illustrator.ShowDetailRepair(repairReport);
                    _illustrator.Pause();

                    continue;
                }

                List<StockItem> stock = BuildStockItems(car);
                DetailType? type = _illustrator.AskReplacementDetail(car, stock, access);

                if (type == null)
                {
                    return _garage.RefuseFurtherRepairs(revenue, car);
                }

                PartCategory category = DetailsCatalog.GetCategory(type.Value);
                DetailRepairReport report = _garage.RepairDetail(type.Value, car, rates.GetRate(category));

                revenue += report.Charge;

                _illustrator.ShowDetailRepair(report);

                if (report.Outcome == RepairOutcome.ReplacedBroken)
                {
                    Unlock(GetCategoryAchievement(category));
                }
                else
                {
                    _illustrator.Pause();
                }
            }

            return new RepairReport(revenue, 0m);
        }

        private void OpenShop()
        {
            bool isOpen = true;

            while (isOpen)
            {
                if (_illustrator.AskShopBrand(out CarBrand brand, out bool isMaterials) == false)
                {
                    isOpen = false;

                    continue;
                }

                OpenPartsShop(brand, isMaterials);
            }
        }

        private void OpenPartsShop(CarBrand brand, bool isMaterials)
        {
            bool isOpen = true;
            List<PartTemplate> templates = BuildShopTemplates(brand, isMaterials);
            string title = GetShopTitle(brand, isMaterials);

            while (isOpen)
            {
                DetailType? type = _illustrator.AskPartType(templates, title, BuildQuantities(templates));

                if (type == null)
                {
                    isOpen = false;

                    continue;
                }

                Brand partBrand = _illustrator.AskBrand(type.Value, _cashRegister.Money);

                if (partBrand == null)
                {
                    continue;
                }

                PurchaseReport report = _partsShop.Buy(type.Value, partBrand);

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
                List<UpgradeOffer> offers = BuildUpgradeOffers();

                UpgradeType? type = _illustrator.AskUpgrade(offers, _cashRegister.Money);

                if (type == null)
                {
                    isOpen = false;

                    continue;
                }

                UpgradeReport report = _upgradeShop.Buy(type.Value);

                if (report.Outcome == UpgradeOutcome.Bought)
                {
                    ApplyUpgradeEffect(report.Type);
                    _dayUpgradeCosts += report.Price;

                    Unlock(AchievementType.FirstUpgrade);

                    if (IsFullyUpgraded())
                    {
                        Unlock(AchievementType.FullUpgrade);
                    }
                }

                _illustrator.ShowUpgradeResult(report, _cashRegister.Money, _warehouse);
                _illustrator.Pause();
            }
        }

        private void Unlock(AchievementType type)
        {
            if (_profile.Unlock(type) == false)
            {
                return;
            }

            _profile.Save();

            _illustrator.ShowAchievement(type);
            _illustrator.Pause();
        }

        private static AchievementType GetCategoryAchievement(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Body:
                    return AchievementType.FirstBodyRepair;

                case PartCategory.Electric:
                    return AchievementType.FirstElectricRepair;

                case PartCategory.Fluid:
                    return AchievementType.FirstFluidChange;

                default:
                    return AchievementType.FirstRepair;
            }
        }

        private bool IsFullyUpgraded()
        {
            foreach (UpgradeInfo info in UpgradeCatalog.All)
            {
                if (_serviceDevelopment.CanUpgrade(info.Type))
                {
                    return false;
                }
            }

            return true;
        }

        private void OpenCourses()
        {
            bool isOpen = true;

            while (isOpen)
            {
                CourseType? type = _illustrator.AskCourse(BuildCourseOffers(), _totalEarned, _cashRegister.Money);

                if (type == null)
                {
                    isOpen = false;

                    continue;
                }

                CourseInfo info = CourseCatalog.Get(type.Value);

                if (_cashRegister.CanAfford(info.Price) == false)
                {
                    _illustrator.ShowCourseResult(info, false, "не хватает денег", _cashRegister.Money);
                    _illustrator.Pause();

                    continue;
                }

                _cashRegister.Pay(info.Price);
                _education.Pass(info.Type);
                _dayUpgradeCosts += info.Price;

                CheckAllCoursesPassed();

                _illustrator.ShowCourseResult(info, true, string.Empty, _cashRegister.Money);
                _illustrator.Pause();

                SaveGame(_day + 1);
            }
        }

        private void CheckAllCoursesPassed()
        {
            foreach (CourseInfo info in CourseCatalog.All)
            {
                if (_education.IsPassed(info.Type) == false)
                {
                    return;
                }
            }

            Unlock(AchievementType.CourseMaster);
        }

        private List<CourseOffer> BuildCourseOffers()
        {
            var offers = new List<CourseOffer>();

            foreach (CourseInfo info in CourseCatalog.All)
            {
                bool isPassed = _education.IsPassed(info.Type);
                bool isAvailable = isPassed == false && _totalEarned >= info.RequiredEarned;

                offers.Add(new CourseOffer(
                    info.Type,
                    info.Name,
                    CarNames.GetBrandTitle(info.Brand),
                    info.RequiredEarned,
                    info.Price,
                    isPassed,
                    isAvailable));
            }

            return offers;
        }

        private List<CarBrand> BuildAvailableBrands()
        {
            var brands = new List<CarBrand> { CarBrand.Vaz };

            foreach (CourseInfo info in CourseCatalog.All)
            {
                if (_education.IsPassed(info.Type))
                {
                    brands.Add(info.Brand);
                }
            }

            return brands;
        }

        private List<StockItem> BuildStockItems(Car car)
        {
            return DetailsCatalog.GetTemplatesFor(car.Brand, car.Name)
                .Where(template => template.Category != PartCategory.Material)
                .Select(template => new StockItem(template, _warehouse.GetQuantityDetail(template.Type)))
                .ToList();
        }

        private List<PartTemplate> BuildShopTemplates(CarBrand brand, bool isMaterials)
        {
            if (isMaterials)
            {
                return DetailsCatalog.GetByCategory(PartCategory.Material);
            }

            var templates = new List<PartTemplate>();

            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(brand))
            {
                if (template.Category != PartCategory.Material)
                {
                    templates.Add(template);
                }
            }

            return templates;
        }

        private static string GetShopTitle(CarBrand brand, bool isMaterials) =>
            isMaterials ? "материалы для покраски" : "запчасти " + CarNames.GetBrandTitle(brand);

        private IReadOnlyDictionary<DetailType, int> BuildQuantities(IReadOnlyList<PartTemplate> templates)
        {
            var quantities = new Dictionary<DetailType, int>();

            foreach (PartTemplate template in templates)
            {
                quantities[template.Type] = _warehouse.GetQuantityDetail(template.Type);
            }

            return quantities;
        }

        private List<UpgradeOffer> BuildUpgradeOffers()
        {
            var offers = new List<UpgradeOffer>();

            foreach (UpgradeInfo info in UpgradeCatalog.All)
            {
                int level = _serviceDevelopment.GetLevel(info.Type);

                offers.Add(new UpgradeOffer(
                    info.Type,
                    info.Name,
                    info.EffectDescription,
                    level,
                    info.MaxLevel,
                    info.HasNextLevel(level) ? info.GetPrice(level) : 0m,
                    _serviceDevelopment.CanUpgrade(info.Type)));
            }

            return offers;
        }

        private WorkRates BuildWorkRates()
        {
            decimal skill = 0.01m * UpgradeCatalog.Get(UpgradeType.Skill).EffectPerLevel
                * _serviceDevelopment.GetLevel(UpgradeType.Skill);

            decimal body = 0.01m * UpgradeCatalog.Get(UpgradeType.BodyBox).EffectPerLevel
                * _serviceDevelopment.GetLevel(UpgradeType.BodyBox);

            decimal electric = 0.01m * UpgradeCatalog.Get(UpgradeType.ElectricBox).EffectPerLevel
                * _serviceDevelopment.GetLevel(UpgradeType.ElectricBox);

            return new WorkRates(skill, body, electric);
        }

        private ServiceAccess BuildServiceAccess()
        {
            bool bodyWorkshop = _serviceDevelopment.GetLevel(UpgradeType.BodyBox) > 0;
            bool bodyBusy = _day < _bodyBoxBusyUntilDay;
            bool electricWorkshop = _serviceDevelopment.GetLevel(UpgradeType.ElectricBox) > 0;

            return new ServiceAccess(bodyWorkshop, electricWorkshop, bodyBusy);
        }

        private string GetBoxStatus()
        {
            if (_serviceDevelopment.GetLevel(UpgradeType.BodyBox) == 0)
            {
                return null;
            }

            return _day < _bodyBoxBusyUntilDay
                ? "Кузовной бокс: занят до дня " + _bodyBoxBusyUntilDay
                : "Кузовной бокс: свободен";
        }

        private void ApplyUpgradeEffect(UpgradeType type)
        {
            if (type == UpgradeType.Warehouse)
            {
                _warehouse.IncreaseCapacity(UpgradeCatalog.Get(type).EffectPerLevel);
            }
        }

        private int GetMaxCarsPerDay()
        {
            UpgradeInfo marketing = UpgradeCatalog.Get(UpgradeType.Marketing);
            int level = _serviceDevelopment.GetLevel(UpgradeType.Marketing);

            return CarsQueue.DefaultMaxCarsPerDay + marketing.EffectPerLevel * level;
        }

        private DayStatistics BuildDayStatistics()
        {
            int cars = _dayReports.Count;
            int repaired = _dayReports.Count(report => report.Fine == 0m);
            int refused = _dayReports.Count(report => report.Fine > 0m);
            decimal fines = _dayReports.Sum(report => report.Fine);
            decimal revenue = _dayReports.Sum(report => report.Revenue);

            return new DayStatistics(
                cars,
                refused,
                repaired,
                revenue,
                fines,
                GameBalance.DailyRent,
                _dayPurchaseCosts,
                _dayUpgradeCosts);
        }

        private ShiftStatistics BuildShiftStatistics()
        {
            return new ShiftStatistics(
                _savedShift.Days + _dayStatistics.Count,
                _savedShift.Cars + _dayStatistics.Sum(statistics => statistics.Cars),
                _savedShift.Repaired + _dayStatistics.Sum(statistics => statistics.Repaired),
                _savedShift.Refused + _dayStatistics.Sum(statistics => statistics.Refused),
                _savedShift.Revenue + _dayStatistics.Sum(statistics => statistics.Revenue),
                _savedShift.Fines + _dayStatistics.Sum(statistics => statistics.Fines),
                _savedShift.Rent + _dayStatistics.Sum(statistics => statistics.Rent),
                _savedShift.PurchaseCosts + _dayStatistics.Sum(statistics => statistics.PurchaseCosts),
                _savedShift.UpgradeCosts + _dayStatistics.Sum(statistics => statistics.UpgradeCosts));
        }

        private GameEndReason? EvaluateDayEnd()
        {
            if (_cashRegister.Money < 0m)
            {
                return GameEndReason.Debt;
            }

            if (_goal.IsReached(_cashRegister.Money))
            {
                return GameEndReason.GoalReached;
            }

            return null;
        }

        private void SaveGame(int nextDay)
        {
            var levels = new Dictionary<UpgradeType, int>();
            var stock = new Dictionary<DetailType, int>();

            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                levels[type] = _serviceDevelopment.GetLevel(type);
            }

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                stock[type] = _warehouse.GetQuantityDetail(type);
            }

            try
            {
                GameSave.Write(
                    _savePath,
                    new GameSave(
                        _cashRegister.Money,
                        nextDay,
                        levels,
                        stock,
                        BuildShiftStatistics(),
                        _bodyBoxBusyUntilDay,
                        _education.Passed,
                        _totalEarned));
            }
            catch (System.IO.IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private GameSave LoadSave()
        {
            if (GameSave.TryRead(_savePath, out GameSave save) == false)
            {
                return null;
            }

            return save;
        }

        private void DeleteSave()
        {
            try
            {
                GameSave.Delete(_savePath);
            }
            catch (System.IO.IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }
}

