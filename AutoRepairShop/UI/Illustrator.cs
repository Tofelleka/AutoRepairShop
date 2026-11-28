using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AutoRepairShop
{
    public enum DayCommand
    {
        EndDay = 0,
        TakeCar = 1,
        Shop = 2,
        Development = 3,
        Achievements = 4,
        Courses = 5,
        Postpone = 6
    }

    internal class StockItem
    {
        public StockItem(PartTemplate template, int quantity)
        {
            Template = template;
            Quantity = quantity;
        }

        public PartTemplate Template { get; }

        public int Quantity { get; }
    }

    internal class UpgradeOffer
    {
        public UpgradeOffer(
            UpgradeType type,
            string name,
            string effectDescription,
            int level,
            int maxLevel,
            decimal price,
            bool canUpgrade)
        {
            Type = type;
            Name = name;
            EffectDescription = effectDescription;
            Level = level;
            MaxLevel = maxLevel;
            Price = price;
            CanUpgrade = canUpgrade;
        }

        public UpgradeType Type { get; }

        public string Name { get; }

        public string EffectDescription { get; }

        public int Level { get; }

        public int MaxLevel { get; }

        public decimal Price { get; }

        public bool CanUpgrade { get; }
    }

    internal class CourseOffer
    {
        public CourseOffer(
            CourseType type,
            string name,
            string brandName,
            decimal requiredEarned,
            decimal price,
            bool isPassed,
            bool isAvailable)
        {
            Type = type;
            Name = name;
            BrandName = brandName;
            RequiredEarned = requiredEarned;
            Price = price;
            IsPassed = isPassed;
            IsAvailable = isAvailable;
        }

        public CourseType Type { get; }

        public string Name { get; }

        public string BrandName { get; }

        public decimal RequiredEarned { get; }

        public decimal Price { get; }

        public bool IsPassed { get; }

        public bool IsAvailable { get; }
    }

    internal class TuningOffer
    {
        public TuningOffer(
            TuningType type,
            string title,
            string description,
            decimal price,
            string partName,
            bool partAvailable)
        {
            Type = type;
            Title = title;
            Description = description;
            Price = price;
            PartName = partName;
            PartAvailable = partAvailable;
        }

        public TuningType Type { get; }

        public string Title { get; }

        public string Description { get; }

        public decimal Price { get; }

        public string PartName { get; }

        public bool PartAvailable { get; }
    }

    internal class Illustrator
    {
        private const int TextWidth = 70;

        private const string Currency = "руб.";

        private const int ExitCommand = 0;
        private const int ConfirmCommand = 1;
        private const int FirstItemNumber = 1;

        private const int TakeCarCommand = 1;
        private const int ShopCommand = 2;
        private const int DevelopmentCommand = 3;
        private const int AchievementsCommand = 4;
        private const int CoursesCommand = 5;
        private const int PostponeCommand = 6;

        public Illustrator()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch (System.IO.IOException)
            {
            }
        }

        public void ShowBanner(decimal startMoney, GameGoal goal, int day, bool isSaveLoaded)
        {
            Clear();

            DoubleLine('╔', '═', '╗');
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("АВТОСЕРВИС"), ConsoleColor.Cyan);
            DoubleRow(Center("«ГАРАЖ НАРОДНЫЙ»"), ConsoleColor.Cyan);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("цель: " + goal.Title + " за " + Money(goal.TargetMoney)), ConsoleColor.White);
            DoubleRow(Center("денег на руках: " + Money(startMoney)), ConsoleColor.White);
            DoubleRow(Center("одна команда — одна деталь"), ConsoleColor.DarkGray);

            if (isSaveLoaded)
            {
                DoubleRow(Center("сохранение загружено, день " + day), ConsoleColor.Green);
            }

            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleLine('╚', '═', '╝');

            Pause();
        }

        public int AskSaveSlot()
        {
            bool isChosen = false;
            int slot = 1;

            while (isChosen == false)
            {
                Clear();

                Top();
                Row("СОХРАНЕНИЯ", ConsoleColor.Cyan);
                Middle();
                Row("1 - слот 1", ConsoleColor.Gray);
                Row("2 - слот 2", ConsoleColor.Gray);
                Row("3 - слот 3", ConsoleColor.Gray);
                Bottom();

                int input = ReadNumber("Слот");

                if (input >= 1 && input <= 3)
                {
                    slot = input;
                    isChosen = true;
                }
                else
                {
                    ShowError("Введите 1, 2 или 3.");
                }
            }

            return slot;
        }

        public void ShowDelivery(int arrived, int pending)
        {
            Top();
            Row("[+] Привезли запчасти: " + arrived + " шт.", ConsoleColor.Green);

            if (pending > 0)
            {
                Row("    ещё в пути: " + pending + " шт.", ConsoleColor.Yellow);
            }

            Bottom();
        }

        public void ShowPostponed(Car car)
        {
            Top();
            Row("[+] Машина отложена до завтра", ConsoleColor.Yellow);
            Row("    " + car.Name, ConsoleColor.White);
            Bottom();
        }

        public void ShowPostponeDenied(string reason)
        {
            Top();
            Row("[-] Нельзя отложить", ConsoleColor.Red);
            Row("    " + reason, ConsoleColor.Gray);
            Bottom();
        }

        public void ShowOwner(Owner owner)
        {
            Top();
            Row("ВЛАДЕЛЕЦ", ConsoleColor.Cyan);
            Row(owner.GetGreeting(), ConsoleColor.Yellow);
            Row(owner.Name + " — " + owner.Model, ConsoleColor.White);
            Row("отношение: " + owner.GetStatus(), owner.IsAngry ? ConsoleColor.Red : ConsoleColor.Yellow);
            Middle();
            WriteWrapped(owner.Story, 2, ConsoleColor.Gray);

            foreach (string fact in owner.Facts)
            {
                WriteWrapped(fact, 2, ConsoleColor.DarkGray);
            }

            Bottom();
        }

        public bool AskDiscount(Owner owner)
        {
            Top();
            Row("СКИДКА", ConsoleColor.Cyan);
            Row(owner.Name + " — " + owner.GetStatus(), ConsoleColor.White);
            Row("Можно дать скидку 10%: отношение улучшится.", ConsoleColor.Gray);
            Middle();
            Row(ConfirmCommand + " - дать скидку", ConsoleColor.Gray);
            Row(ExitCommand + " - без скидки", ConsoleColor.DarkGray);
            Bottom();

            return AskYesNo();
        }

        public void ShowDiscountApplied(Owner owner, decimal amount)
        {
            Top();
            Row("[+] Скидка " + Money(amount), ConsoleColor.Yellow);
            Row("    " + owner.Name + ": " + owner.GetStatus(), ConsoleColor.White);
            WriteWrapped(Accents.GetDiscountThanks(owner.Nationality), 4, ConsoleColor.Gray);
            Bottom();
        }

        public void ShowOwnerReaction(Owner owner, int delta)
        {
            Top();

            if (delta >= 0)
            {
                Row("[+] Владелец доволен работой", ConsoleColor.Green);
            }
            else
            {
                Row("[-] Владелец недоволен", ConsoleColor.Red);
            }

            Row("    " + owner.Name + ": " + owner.GetStatus(), ConsoleColor.White);
            WriteWrapped(delta >= 0 ? owner.GetPraise() : owner.GetComplaint(), 4, ConsoleColor.Gray);

            if (owner.IsAngry)
            {
                Row("    он в ярости и больше сюда не приедет", ConsoleColor.Red);
            }
            else
            {
                WriteWrapped(owner.GetFarewell(), 4, ConsoleColor.DarkGray);
            }

            Bottom();
        }

        public void ShowReferral(Owner friend, Owner newcomer)
        {
            Top();
            Row("[*] Сарафанное радио", ConsoleColor.Magenta);
            Row("    " + friend.Name + " привёл знакомого:", ConsoleColor.Gray);
            Row("    " + newcomer.GetGreeting() + " " + newcomer.Name, ConsoleColor.White);
            Row("    " + newcomer.Model, ConsoleColor.Gray);
            Bottom();
        }

        public void ShowAcquaintanceLost(Owner friend, Owner newcomer)
        {
            Top();
            Row("[-] Друг обиделся и увёл знакомого", ConsoleColor.Red);
            Row("    " + friend.Name + " больше не приведёт " + newcomer.Name, ConsoleColor.Gray);
            Bottom();
        }

        public void ShowAchievements(Profile profile)
        {
            Clear();

            Top();
            Row("ДОСТИЖЕНИЯ", ConsoleColor.Cyan);
            Middle();

            foreach (AchievementType type in Enum.GetValues(typeof(AchievementType)))
            {
                bool isUnlocked = profile.IsUnlocked(type);

                string line = (isUnlocked ? "[+] " : "[ ] ") + Achievements.GetTitle(type);

                Row(line, isUnlocked ? ConsoleColor.Green : ConsoleColor.DarkGray);
            }

            Middle();
            Row(TwoColumns("рекорд кассы:", Money(profile.BestMoney)), ConsoleColor.White);
            Bottom();

            Pause();
        }

        public CourseType? AskCourse(IReadOnlyList<CourseOffer> offers, decimal earned, decimal money)
        {
            bool isChosen = false;
            CourseType? type = null;

            while (isChosen == false)
            {
                DrawCourses(offers, earned, money);

                int input = ReadNumber("Курс");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= offers.Count)
                {
                    CourseOffer offer = offers[input - FirstItemNumber];

                    if (offer.IsPassed)
                    {
                        ShowError("Этот курс уже пройден.");
                    }
                    else if (offer.IsAvailable == false)
                    {
                        string reason = earned < offer.RequiredEarned
                            ? "Курс откроется после " + Money(offer.RequiredEarned) + " накоплений."
                            : "Не хватает денег на курс.";

                        ShowError(reason);
                    }
                    else
                    {
                        type = offer.Type;
                        isChosen = true;
                    }
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        public void ShowCourseResult(CourseInfo info, bool isPassed, string reason, decimal money)
        {
            Top();

            if (isPassed)
            {
                Row("[+] Курс пройден: " + info.Name, ConsoleColor.Green);
                Row("    марка " + CarNames.GetBrandTitle(info.Brand) + " теперь приезжает", ConsoleColor.Gray);
                Row("    в кассе: " + Money(money), ConsoleColor.White);
            }
            else
            {
                Row("[-] " + info.Name + ": " + reason, ConsoleColor.Red);
                Row("    в кассе: " + Money(money), ConsoleColor.Red);
            }

            Bottom();
        }

        private void DrawCourses(IReadOnlyList<CourseOffer> offers, decimal earned, decimal money)
        {
            Clear();

            Top();
            Row("КУРСЫ ПРОФПОДГОТОВКИ", ConsoleColor.Cyan);
            Row(TwoColumns("накоплено:", Money(earned)), ConsoleColor.White);
            Row(TwoColumns("в кассе:", Money(money)), ConsoleColor.White);
            Middle();
            Row(TwoColumns("курс", "цена"), ConsoleColor.DarkGray);

            for (int i = 0; i < offers.Count; i++)
            {
                CourseOffer offer = offers[i];

                string status = offer.IsPassed
                    ? "пройден"
                    : offer.IsAvailable ? Money(offer.Price) : "нужно " + Money(offer.RequiredEarned);

                string left = (i + FirstItemNumber) + ". " + offer.Name + " (" + offer.BrandName + ")";

                Row(TwoColumns(left, status),
                    offer.IsPassed ? ConsoleColor.DarkGray : offer.IsAvailable ? ConsoleColor.Gray : ConsoleColor.DarkYellow);
            }

            Middle();
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        public bool AskContinueGame()
        {
            Clear();

            Top();
            Row("СОХРАНЕНИЕ НАЙДЕНО", ConsoleColor.Cyan);
            Middle();
            Row(ConfirmCommand + " - продолжить", ConsoleColor.Gray);
            Row(ExitCommand + " - начать заново", ConsoleColor.DarkGray);
            Bottom();

            bool isAnswered = false;
            bool isContinue = false;

            while (isAnswered == false)
            {
                int input = ReadNumber("Решение");

                if (input == ConfirmCommand)
                {
                    isContinue = true;
                    isAnswered = true;
                }
                else if (input == ExitCommand)
                {
                    isContinue = false;
                    isAnswered = true;
                }
                else
                {
                    ShowError("Введите 1 или 0.");
                }
            }

            return isContinue;
        }

        public DayCommand AskDayCommand(int day, decimal money, int carsInQueue, GameGoal goal, string boxStatus, string seasonText)
        {
            DrawDayScreen(day, money, carsInQueue, goal, boxStatus, seasonText);

            bool isChosen = false;
            DayCommand command = DayCommand.EndDay;

            while (isChosen == false)
            {
                int input = ReadNumber("Команда");

                switch (input)
                {
                    case TakeCarCommand:
                        command = DayCommand.TakeCar;
                        isChosen = true;
                        break;

                    case ShopCommand:
                        command = DayCommand.Shop;
                        isChosen = true;
                        break;

                    case DevelopmentCommand:
                        command = DayCommand.Development;
                        isChosen = true;
                        break;

                    case AchievementsCommand:
                        command = DayCommand.Achievements;
                        isChosen = true;
                        break;

                    case CoursesCommand:
                        command = DayCommand.Courses;
                        isChosen = true;
                        break;

                    case PostponeCommand:
                        command = DayCommand.Postpone;
                        isChosen = true;
                        break;

                    case ExitCommand:
                        command = DayCommand.EndDay;
                        isChosen = true;
                        break;

                    default:
                        ShowError("Такой команды нет. Введите 1, 2, 3 или 0.");
                        break;
                }
            }

            return command;
        }

        public void ShowNoCars()
        {
            Clear();

            Top();
            Row("Очередь пуста.", ConsoleColor.DarkGray);
            Row("Новые машины приедут на следующий день.", ConsoleColor.DarkGray);
            Bottom();
        }

        public bool AskRepairCar(Car car)
        {
            DrawCarScreen(car);

            bool isAnswered = false;
            bool isRepair = false;

            while (isAnswered == false)
            {
                int input = ReadNumber("Решение");

                if (input == ConfirmCommand)
                {
                    isRepair = true;
                    isAnswered = true;
                }
                else if (input == ExitCommand)
                {
                    isRepair = false;
                    isAnswered = true;
                }
                else
                {
                    ShowError("Введите 1 или 0.");
                }
            }

            return isRepair;
        }

        public RepairAction AskRepairAction(
            Car car,
            IReadOnlyList<Detail> brokenDetails,
            WorkRates rates,
            ServiceAccess access,
            bool hasTireJob)
        {
            if ((brokenDetails == null || brokenDetails.Count == 0) && hasTireJob == false)
            {
                DrawEachDetailIsFineScreen(car);

                return RepairAction.Cancel;
            }

            DrawChecklist(car, brokenDetails, rates, access, hasTireJob);

            bool isChosen = false;
            RepairAction action = RepairAction.Cancel;

            while (isChosen == false)
            {
                int input = ReadNumber("Действие");

                switch (input)
                {
                    case ExitCommand:
                        action = RepairAction.Cancel;
                        isChosen = true;
                        break;

                    case 1:
                        action = RepairAction.Replace;
                        isChosen = true;
                        break;

                    case 2:
                        if (HasRepairable(brokenDetails, access))
                        {
                            action = RepairAction.Repair;
                            isChosen = true;
                        }
                        else
                        {
                            ShowError("Ремонт без замены сейчас недоступен.");
                        }

                        break;

                    case 3:
                        if (hasTireJob)
                        {
                            action = RepairAction.TireService;
                            isChosen = true;
                        }
                        else
                        {
                            ShowError("Шиномонтаж сейчас не нужен.");
                        }

                        break;

                    default:
                        ShowError("Введите 0, 1, 2 или 3.");
                        break;
                }
            }

            return action;
        }

        public DetailType? AskReplacementDetail(Car car, IReadOnlyList<StockItem> stock, ServiceAccess access) =>
            AskStockDetail(car, stock, access);

        public DetailType? AskRepairableDetail(Car car, IReadOnlyList<Detail> repairable, WorkRates rates)
        {
            DrawRepairableScreen(car, repairable, rates);

            bool isChosen = false;
            DetailType? type = null;

            while (isChosen == false)
            {
                int input = ReadNumber("Ремонт");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= repairable.Count)
                {
                    type = repairable[input - FirstItemNumber].Type;
                    isChosen = true;
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        private static bool HasRepairable(IReadOnlyList<Detail> brokenDetails, ServiceAccess access)
        {
            foreach (Detail detail in brokenDetails)
            {
                if (detail.IsRepairable && access.CanRepair(detail.Category))
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawRepairableScreen(Car car, IReadOnlyList<Detail> repairable, WorkRates rates)
        {
            Clear();

            Top();
            Row("РЕМОНТ БЕЗ ЗАМЕНЫ", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Row(TwoColumns("можно отремонтировать:", repairable.Count + " шт."), ConsoleColor.Gray);
            Middle();
            Row(TwoColumns("деталь", "работа"), ConsoleColor.DarkGray);

            for (int i = 0; i < repairable.Count; i++)
            {
                Detail detail = repairable[i];

                string left = (i + FirstItemNumber) + ". " + detail.Name + " (" + detail.BrandName + ")";
                string right = Money(detail.GetRepairCharge(rates.GetRate(detail.Category)));

                Row(TwoColumns(left, right), ConsoleColor.Gray);
            }

            Middle();
            Row("деталь остаётся на машине, со склада ничего не тратится", ConsoleColor.Yellow);
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        public TuningType? AskTuning(Car car, IReadOnlyList<TuningOffer> offers)
        {
            bool isChosen = false;
            TuningType? type = null;

            while (isChosen == false)
            {
                DrawTuningScreen(car, offers);

                int input = ReadNumber("Тюнинг");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= offers.Count)
                {
                    TuningOffer offer = offers[input - FirstItemNumber];

                    if (offer.PartAvailable == false)
                    {
                        ShowError("Нужной тюнинг-детали нет на складе — закажите её в магазине.");
                    }
                    else
                    {
                        type = offer.Type;
                        isChosen = true;
                    }
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        public void ShowTuningDone(TuningOption option)
        {
            Top();
            Row("[+] Тюнинг выполнен: " + option.Title, ConsoleColor.Green);
            Row("    оплата: " + Money(option.Price), ConsoleColor.Green);
            Row("    деталь ушла со склада: " + option.PartType, ConsoleColor.DarkGray);
            Bottom();
        }

        public void ShowTuningFailed(string reason)
        {
            Top();
            Row("[-] Тюнинг не выполнен", ConsoleColor.Red);
            Row("    " + reason, ConsoleColor.Gray);
            Bottom();
        }

        public void ShowTireChangeDone(decimal price)
        {
            Top();
            Row("[+] Шиномонтаж выполнен", ConsoleColor.Green);
            Row("    оплата: " + Money(price), ConsoleColor.Green);
            Row("    шины клиента, со склада ничего не тратится", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawTuningScreen(Car car, IReadOnlyList<TuningOffer> offers)
        {
            Clear();

            Top();
            Row("ТЮНИНГ: " + car.Name, ConsoleColor.Cyan);
            Middle();
            Row(TwoColumns("вариант", "цена"), ConsoleColor.DarkGray);

            for (int i = 0; i < offers.Count; i++)
            {
                TuningOffer offer = offers[i];

                string left = (i + FirstItemNumber) + ". " + offer.Title;
                string right = Money(offer.Price) + (offer.PartAvailable ? string.Empty : " (нет детали)");

                Row(TwoColumns(left, right), offer.PartAvailable ? ConsoleColor.Gray : ConsoleColor.DarkGray);
                WriteWrapped(offer.Description, 3, ConsoleColor.DarkGray);
                Row("   деталь: " + offer.PartName, ConsoleColor.DarkGray);
            }

            Middle();
            Row(ExitCommand + " - без тюнинга", ConsoleColor.DarkGray);
            Bottom();
        }

        public void ShowDetailRepair(DetailRepairReport report)
        {
            Top();

            switch (report.Outcome)
            {
                case RepairOutcome.ReplacedBroken:
                    Row("[+] " + DetailName(report.Detail) + " заменена", ConsoleColor.Green);
                    Row("    выплата: " + Money(report.Charge), ConsoleColor.Green);
                    break;

                case RepairOutcome.ReplacedHealthy:
                    Row("[!] " + DetailName(report.Detail) + " была исправна", ConsoleColor.Yellow);
                    Row("    деталь списана со склада, выплаты нет", ConsoleColor.Yellow);
                    break;

                case RepairOutcome.NoDetailInStock:
                    Row("[-] На складе нет детали: " + DetailName(report.Detail), ConsoleColor.Red);
                    break;

                case RepairOutcome.Repaired:
                    Row("[+] Ремонт выполнен: " + DetailName(report.Detail), ConsoleColor.Green);
                    Row("    работа: " + Money(report.Charge), ConsoleColor.Green);
                    break;

                case RepairOutcome.NotRepairable:
                    Row("[-] Эту деталь нельзя отремонтировать без замены", ConsoleColor.Red);
                    Row("    придётся ставить новую: " + DetailName(report.Detail), ConsoleColor.DarkGray);
                    break;

                default:
                    Row("[!] Неизвестный исход ремонта", ConsoleColor.Yellow);
                    break;
            }

            if (report.RelatedBroken != null)
            {
                Row("[!] Дешёвая жидкость повредила: " + report.RelatedBroken.Name, ConsoleColor.Yellow);
            }

            Bottom();
        }

        public void ShowRepairReport(RepairReport report, decimal money)
        {
            Top();
            Row("ИТОГ РЕМОНТА", ConsoleColor.Cyan);
            Middle();
            Row(TwoColumns("Выручка:", Money(report.Revenue)), ConsoleColor.Gray);
            Row(TwoColumns("Штраф:", Money(report.Fine)),
                report.Fine > 0m ? ConsoleColor.Red : ConsoleColor.DarkGray);
            Row(TwoColumns("Итого:", Money(report.Total)),
                report.Total >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            Middle();
            Row(TwoColumns("В кассе:", Money(money)),
                money >= 0m ? ConsoleColor.White : ConsoleColor.Red);
            Bottom();
        }

        public bool AskShopBrand(out CarBrand brand, out bool isMaterials, out bool isTuning)
        {
            brand = CarBrand.Vaz;
            isMaterials = false;
            isTuning = false;

            bool isChosen = false;
            bool isCancelled = false;

            while (isChosen == false)
            {
                DrawShopBrands();

                int input = ReadNumber("Магазин");

                switch (input)
                {
                    case 1:
                        brand = CarBrand.Vaz;
                        isChosen = true;
                        break;

                    case 2:
                        brand = CarBrand.Izh;
                        isChosen = true;
                        break;

                    case 3:
                        brand = CarBrand.Volga;
                        isChosen = true;
                        break;

                    case 4:
                        isMaterials = true;
                        isChosen = true;
                        break;

                    case 5:
                        isTuning = true;
                        isChosen = true;
                        break;

                    case ExitCommand:
                        isCancelled = true;
                        isChosen = true;
                        break;

                    default:
                        ShowError("Введите 1-5 или 0.");
                        break;
                }
            }

            return isCancelled == false;
        }

        private void DrawShopBrands()
        {
            Clear();

            Top();
            Row("МАГАЗИНЫ", ConsoleColor.Cyan);
            Middle();
            Row("1 - ВАЗ", ConsoleColor.Gray);
            Row("2 - Иж", ConsoleColor.Gray);
            Row("3 - Волга", ConsoleColor.Gray);
            Row("4 - материалы для покраски", ConsoleColor.Gray);
            Row("5 - тюнинг-детали", ConsoleColor.Gray);
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        public DetailType? AskPartType(
            IReadOnlyList<PartTemplate> templates,
            string title,
            IReadOnlyDictionary<DetailType, int> quantities)
        {
            bool isChosen = false;
            DetailType? type = null;

            while (isChosen == false)
            {
                DrawPartList(title, templates, quantities);

                int input = ReadNumber("Деталь");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= templates.Count)
                {
                    type = templates[input - FirstItemNumber].Type;
                    isChosen = true;
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        public Brand AskBrand(DetailType type, decimal money)
        {
            PartTemplate template = DetailsCatalog.GetTemplate(type);

            bool isChosen = false;
            Brand brand = null;

            while (isChosen == false)
            {
                DrawBrandList(template, money);

                int input = ReadNumber("Производитель");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= DetailsCatalog.Brands.Count)
                {
                    Brand selected = DetailsCatalog.Brands[input - FirstItemNumber];

                    if (ConfirmBrand(template, selected, money))
                    {
                        brand = selected;
                        isChosen = true;
                    }
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return brand;
        }

        public void ShowPurchase(PurchaseReport report, decimal money)
        {
            Top();

            switch (report.Outcome)
            {
                case PurchaseOutcome.Bought:
                    Row("[+] Заказано: " + DetailName(report.Detail) + " (" + BrandName(report.Detail) + ")",
                        ConsoleColor.Green);
                    Row("    оплачено: " + Money(report.Detail.Price), ConsoleColor.Green);
                    Row("    приедет завтра на склад", ConsoleColor.Yellow);
                    Row("    в кассе: " + Money(money), ConsoleColor.White);
                    break;

                case PurchaseOutcome.NotEnoughMoney:
                    Row("[-] Не хватает денег", ConsoleColor.Red);
                    Row("    нужно: " + Money(report.Detail.Price) + ", в кассе: " + Money(money), ConsoleColor.Red);
                    break;

                case PurchaseOutcome.WarehouseIsFull:
                    Row("[!] Склад полон", ConsoleColor.Yellow);
                    break;

                default:
                    Row("[!] Такой детали нет в каталоге", ConsoleColor.Yellow);
                    break;
            }

            Bottom();
        }

        public bool AskNextDay(int day, decimal money)
        {
            DrawNextDayScreen(day, money);

            bool isAnswered = false;
            bool isContinue = false;

            while (isAnswered == false)
            {
                int input = ReadNumber("Решение");

                if (input == ConfirmCommand)
                {
                    isContinue = true;
                    isAnswered = true;
                }
                else if (input == ExitCommand)
                {
                    isContinue = false;
                    isAnswered = true;
                }
                else
                {
                    ShowError("Введите 1 или 0.");
                }
            }

            return isContinue;
        }

        public bool AskRepaint(Car car, decimal price, int days, PaintColor color)
        {
            Clear();

            Top();
            Row("ЗАКАЗ НА ПЕРЕКРАС", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Middle();
            Row("Клиент просит полный перекрас кузова.", ConsoleColor.Gray);
            Row(TwoColumns("заказанный цвет:", PaintColors.GetName(color)), ConsoleColor.White);
            Row(TwoColumns("оплата:", Money(price)), ConsoleColor.Green);
            Row(TwoColumns("бокс занят:", days + " дн."), ConsoleColor.Yellow);
            Row("нужны: краска заказанного цвета, грунт и лак", ConsoleColor.DarkGray);
            Middle();
            Row(ConfirmCommand + " - взять заказ", ConsoleColor.Gray);
            Row(ExitCommand + " - отказать", ConsoleColor.DarkGray);
            Bottom();

            return AskYesNo();
        }

        public PaintColor? AskPaintColor(IReadOnlyDictionary<DetailType, int> paints)
        {
            bool isChosen = false;
            PaintColor? color = null;

            while (isChosen == false)
            {
                DrawPaintList(paints);

                int input = ReadNumber("Краска");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= PaintColors.All.Count)
                {
                    color = PaintColors.All[input - FirstItemNumber];
                    isChosen = true;
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return color;
        }

        public void ShowRepaintFailed(PaintColor ordered, PaintColor used)
        {
            Top();
            Row("[-] Перекрас не принят клиентом", ConsoleColor.Red);
            Row("    заказан: " + PaintColors.GetName(ordered), ConsoleColor.Gray);
            Row("    покрашено: " + PaintColors.GetName(used), ConsoleColor.Red);
            Row("    материалы израсходованы, оплаты нет", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawPaintList(IReadOnlyDictionary<DetailType, int> paints)
        {
            Clear();

            Top();
            Row("ВЫБОР КРАСКИ", ConsoleColor.Cyan);
            Row("Нужны краска нужного цвета, грунт и лак.", ConsoleColor.DarkGray);
            Middle();
            Row(TwoColumns("цвет", "на складе"), ConsoleColor.DarkGray);

            for (int i = 0; i < PaintColors.All.Count; i++)
            {
                PaintColor color = PaintColors.All[i];
                DetailType type = PaintColors.ToDetailType(color);
                int quantity = paints.TryGetValue(type, out int count) ? count : 0;

                string left = (i + FirstItemNumber) + ". " + PaintColors.GetName(color);
                string right = quantity + " шт.";

                Row(TwoColumns(left, right), quantity > 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray);
            }

            Middle();
            Row(ExitCommand + " - отменить заказ", ConsoleColor.DarkGray);
            Bottom();
        }

        public void ShowRepaintDone(decimal price, int busyUntilDay)
        {
            Top();
            Row("[+] Перекрас выполнен", ConsoleColor.Green);
            Row("    оплата: " + Money(price), ConsoleColor.Green);
            Row("    кузовной бокс занят до дня " + busyUntilDay, ConsoleColor.Yellow);
            Bottom();
        }

        public void ShowAchievement(AchievementType type)
        {
            Top();
            Row("[*] Достижение: " + Achievements.GetTitle(type), ConsoleColor.Magenta);
            Bottom();
        }

        public void ShowFinal(decimal money, ShiftStatistics statistics, GameEndReason reason, Profile profile)
        {
            Clear();

            DoubleLine('╔', '═', '╗');
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center(GetFinalTitle(reason)), GetFinalColor(reason));
            DoubleRow(Center(GetFinalMessage(reason)), GetFinalColor(reason));
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("дней отработано: " + statistics.Days), ConsoleColor.White);
            DoubleRow(Center("в кассе: " + Money(money)),
                money >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            DoubleLine('╠', '═', '╣');
            DoubleRow(" машин обслужено: " + statistics.Cars, ConsoleColor.Gray);
            DoubleRow("   отремонтировано: " + statistics.Repaired, ConsoleColor.Green);
            DoubleRow("   отказов: " + statistics.Refused, ConsoleColor.Gray);
            DoubleRow(" выручка: " + Money(statistics.Revenue), ConsoleColor.Gray);
            DoubleRow(" штрафы: " + Money(statistics.Fines), ConsoleColor.Red);
            DoubleRow(" аренда: " + Money(statistics.Rent), ConsoleColor.Red);
            DoubleRow(" закупки: " + Money(statistics.PurchaseCosts), ConsoleColor.Red);
            DoubleRow(" улучшения: " + Money(statistics.UpgradeCosts), ConsoleColor.Red);
            DoubleRow(" сарафанное радио: " + statistics.Referrals + " чел.", ConsoleColor.Magenta);
            DoubleRow(" итог смены: " + Money(statistics.Profit),
                statistics.Profit >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            DoubleLine('╠', '═', '╣');
            DoubleRow(" рекорд кассы: " + Money(profile.BestMoney), ConsoleColor.White);
            DoubleRow(" достижений: " + profile.Achievements.Count, ConsoleColor.Magenta);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleLine('╚', '═', '╝');

            Pause();
        }

        public UpgradeType? AskUpgrade(IReadOnlyList<UpgradeOffer> offers, decimal money)
        {
            bool isChosen = false;
            UpgradeType? type = null;

            while (isChosen == false)
            {
                DrawDevelopmentScreen(offers, money);

                int input = ReadNumber("Улучшение");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= offers.Count)
                {
                    UpgradeOffer offer = offers[input - FirstItemNumber];

                    if (offer.CanUpgrade == false)
                    {
                        ShowError("Улучшение уже максимального уровня.");
                    }
                    else if (ConfirmUpgrade(offer, money))
                    {
                        type = offer.Type;
                        isChosen = true;
                    }
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        public void ShowUpgradeResult(UpgradeReport report, decimal money, Warehouse warehouse)
        {
            Top();

            switch (report.Outcome)
            {
                case UpgradeOutcome.Bought:
                    Row("[+] " + report.Name + ": уровень " + report.Level, ConsoleColor.Green);

                    if (report.Type == UpgradeType.Warehouse)
                    {
                        Row("    теперь мест на складе: " + warehouse.Capacity, ConsoleColor.Green);
                    }

                    Row("    потрачено: " + Money(report.Price), ConsoleColor.Green);
                    Row("    в кассе: " + Money(money), ConsoleColor.White);
                    break;

                case UpgradeOutcome.NotEnoughMoney:
                    Row("[-] Не хватает денег на: " + report.Name, ConsoleColor.Red);
                    Row("    нужно: " + Money(report.Price) + ", в кассе: " + Money(money), ConsoleColor.Red);
                    break;

                default:
                    Row("[!] " + report.Name + " уже максимального уровня", ConsoleColor.Yellow);
                    break;
            }

            Bottom();
        }

        public void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Нажмите Enter, чтобы продолжить...");
            Console.ResetColor();
            Console.ReadLine();
        }

        public void ShowDayStatistics(DayStatistics statistics)
        {
            Clear();

            Top();
            Row("ИТОГИ ДНЯ", ConsoleColor.Cyan);
            Middle();
            Row(TwoColumns("Машин обслужено:", statistics.Cars + " шт."), ConsoleColor.Gray);
            Row(TwoColumns("Отремонтировано полностью:", statistics.Repaired + " шт."), ConsoleColor.Green);
            Row(TwoColumns("Отказов и прерываний:", statistics.Refused + " шт."), ConsoleColor.DarkGray);
            Middle();
            Row(TwoColumns("Выручка:", Money(statistics.Revenue)), ConsoleColor.Gray);
            Row(TwoColumns("Штрафы:", Money(statistics.Fines)),
                statistics.Fines > 0m ? ConsoleColor.Red : ConsoleColor.DarkGray);
            Row(TwoColumns("Чистая выручка:", Money(statistics.NetRevenue)),
                statistics.NetRevenue >= 0m ? ConsoleColor.White : ConsoleColor.Red);
            Middle();
            Row(TwoColumns("Аренда:", Money(statistics.Rent)), ConsoleColor.Red);
            Row(TwoColumns("Закупки:", Money(statistics.PurchaseCosts)),
                statistics.PurchaseCosts > 0m ? ConsoleColor.Red : ConsoleColor.DarkGray);
            Row(TwoColumns("Улучшения:", Money(statistics.UpgradeCosts)),
                statistics.UpgradeCosts > 0m ? ConsoleColor.Red : ConsoleColor.DarkGray);
            Row(TwoColumns("Прибыль:", Money(statistics.Profit)),
                statistics.Profit >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            Bottom();
        }

        private void DrawDayScreen(int day, decimal money, int carsInQueue, GameGoal goal, string boxStatus, string seasonText)
        {
            Clear();

            Top();
            Row("ДЕНЬ " + day, ConsoleColor.Cyan);
            Row(seasonText, ConsoleColor.DarkYellow);
            Row(TwoColumns("Цель: " + goal.Title,
                Money(goal.TargetMoney) + " (" + goal.GetProgress(money) + "%)"), ConsoleColor.Cyan);
            Row(TwoColumns("В кармане:", Money(money)), ConsoleColor.White);
            Row(TwoColumns("В очереди машин:", carsInQueue + " шт."),
                carsInQueue > 0 ? ConsoleColor.Green : ConsoleColor.DarkGray);

            if (string.IsNullOrEmpty(boxStatus) == false)
            {
                Row(boxStatus, ConsoleColor.Yellow);
            }

            Middle();
            Row(TakeCarCommand + " - принять машину",
                carsInQueue > 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray);
            Row(ShopCommand + " - магазин запчастей", ConsoleColor.Gray);
            Row(DevelopmentCommand + " - развитие сервиса", ConsoleColor.Gray);
            Row(AchievementsCommand + " - достижения", ConsoleColor.Gray);
            Row(CoursesCommand + " - курсы профподготовки", ConsoleColor.Gray);
            Row(PostponeCommand + " - отложить машину на завтра", ConsoleColor.Gray);
            Row(ExitCommand + " - закончить день", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawDevelopmentScreen(IReadOnlyList<UpgradeOffer> offers, decimal money)
        {
            Clear();

            Top();
            Row("РАЗВИТИЕ СЕРВИСА", ConsoleColor.Cyan);
            Row(TwoColumns("В кассе:", Money(money)), ConsoleColor.White);
            Middle();
            Row(TwoColumns("улучшение", "цена"), ConsoleColor.DarkGray);

            for (int i = 0; i < offers.Count; i++)
            {
                UpgradeOffer offer = offers[i];

                string left = (i + FirstItemNumber) + ". " + offer.Name
                    + " [" + offer.Level + "/" + offer.MaxLevel + "] " + offer.EffectDescription;
                string right = offer.CanUpgrade ? Money(offer.Price) : "максимум";

                Row(TwoColumns(left, right), offer.CanUpgrade ? ConsoleColor.Gray : ConsoleColor.DarkGray);
            }

            Middle();
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        private bool ConfirmUpgrade(UpgradeOffer offer, decimal money)
        {
            Clear();

            Top();
            Row("УЛУЧШЕНИЕ", ConsoleColor.Cyan);
            Row(offer.Name, ConsoleColor.White);
            Middle();
            Row(TwoColumns("Эффект:", offer.EffectDescription), ConsoleColor.Gray);
            Row(TwoColumns("Уровень:", offer.Level + " -> " + (offer.Level + 1)), ConsoleColor.Gray);
            Row(TwoColumns("Цена:", Money(offer.Price)), ConsoleColor.White);
            Row(TwoColumns("В кассе:", Money(money)),
                money >= offer.Price ? ConsoleColor.Gray : ConsoleColor.Red);
            Middle();
            Row(ConfirmCommand + " - купить", ConsoleColor.Gray);
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();

            bool isAnswered = false;
            bool isBuy = false;

            while (isAnswered == false)
            {
                int input = ReadNumber("Решение");

                if (input == ConfirmCommand)
                {
                    isBuy = true;
                    isAnswered = true;
                }
                else if (input == ExitCommand)
                {
                    isBuy = false;
                    isAnswered = true;
                }
                else
                {
                    ShowError("Введите 1 или 0.");
                }
            }

            return isBuy;
        }

        private void DrawCarScreen(Car car)
        {
            Clear();

            Top();
            Row("МАШИНА НА ПОДЪЁМНИКЕ", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Middle();
            Row("Ремонтируем?", ConsoleColor.Gray);
            Row(ConfirmCommand + " - да, берём в работу", ConsoleColor.Gray);
            Row(ExitCommand + " - отказать (фиксированный штраф)", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawEachDetailIsFineScreen(Car car)
        {
            Clear();

            Top();
            Row("МАШИНА ИСПРАВНА", ConsoleColor.Green);
            Row(car.Name, ConsoleColor.White);
            Bottom();
        }

        private void DrawChecklist(Car car, IReadOnlyList<Detail> brokenDetails, WorkRates rates, ServiceAccess access, bool hasTireJob)
        {
            Clear();

            Top();
            Row("ЧЕК-ЛИСТ ПОЧИНКИ", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Row(TwoColumns("сломано деталей:", brokenDetails.Count + " шт."), ConsoleColor.Gray);
            Middle();
            Row(TwoColumns("деталь", "выплата"), ConsoleColor.DarkGray);
            Middle();

            foreach (Detail detail in brokenDetails)
            {
                bool isLocked = access.CanRepair(detail.Category) == false;

                string name = "  " + detail.Name + " (" + detail.BrandName + ")"
                    + (isLocked ? " — " + access.GetLockReason(detail.Category) : string.Empty);

                Row(TwoColumns(name, Money(detail.GetPayout(rates.GetRate(detail.Category)))),
                    isLocked ? ConsoleColor.DarkGray : ConsoleColor.Gray);

                WriteWrapped(detail.Description, 4, ConsoleColor.DarkGray);
            }

            if (hasTireJob)
            {
                Row(TwoColumns("  Шиномонтаж (шины клиента)", Money(GameBalance.TireChangePrice)), ConsoleColor.Yellow);
            }

            Middle();
            Row("1 - заменить деталь", ConsoleColor.Gray);
            Row("2 - отремонтировать без замены", ConsoleColor.Gray);

            if (hasTireJob)
            {
                Row("3 - шиномонтаж", ConsoleColor.Gray);
            }

            Row(ExitCommand + " - прекратить ремонт", ConsoleColor.DarkGray);
            Bottom();
        }

        private DetailType? AskStockDetail(Car car, IReadOnlyList<StockItem> stock, ServiceAccess access)
        {
            DrawStockScreen(car, stock, access);

            bool isChosen = false;
            DetailType? type = null;

            while (isChosen == false)
            {
                int input = ReadNumber("Деталь");

                if (input == ExitCommand)
                {
                    isChosen = true;
                }
                else if (input >= FirstItemNumber && input <= stock.Count)
                {
                    StockItem item = stock[input - 1];

                    if (access.CanRepair(item.Template.Category) == false)
                    {
                        ShowError("Для этой категории нужен бокс — купите его в развитии.");
                    }
                    else
                    {
                        type = item.Template.Type;
                        isChosen = true;
                    }
                }
                else
                {
                    ShowError("Такой позиции нет в списке.");
                }
            }

            return type;
        }

        private void DrawStockScreen(Car car, IReadOnlyList<StockItem> stock, ServiceAccess access)
        {
            Clear();

            Top();
            Row("ЗАМЕНА ДЕТАЛИ", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Row("Выберите деталь, которую поставим на машину.", ConsoleColor.Gray);
            Middle();
            Row(TwoColumns("деталь", "цена от (на складе)"), ConsoleColor.DarkGray);

            for (int i = 0; i < stock.Count; i++)
            {
                StockItem item = stock[i];
                bool isLocked = access.CanRepair(item.Template.Category) == false;

                string left = (i + FirstItemNumber) + ". " + item.Template.Name
                    + " [" + CategoryName(item.Template.Category) + "]";
                string right = Money(item.Template.GetPrice(DetailsCatalog.StandardBrand))
                    + " (" + item.Quantity + " шт.)";

                Row(TwoColumns(left, right),
                    isLocked || item.Quantity == 0 ? ConsoleColor.DarkGray : ConsoleColor.Gray);
            }

            Middle();
            Row("Если деталь на машине исправна — она уйдёт со склада без выплаты.", ConsoleColor.Yellow);
            Row(ExitCommand + " - прекратить ремонт", ConsoleColor.DarkGray);
            Row("(штраф за каждую непочиненную деталь)", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawPartList(string title, IReadOnlyList<PartTemplate> templates, IReadOnlyDictionary<DetailType, int> quantities)
        {
            Clear();

            Top();
            Row("МАГАЗИН: " + title, ConsoleColor.Cyan);
            Middle();
            Row(TwoColumns("деталь", "цена от (на складе)"), ConsoleColor.DarkGray);

            for (int i = 0; i < templates.Count; i++)
            {
                PartTemplate template = templates[i];
                int quantity = quantities.TryGetValue(template.Type, out int count) ? count : 0;

                string left = (i + FirstItemNumber) + ". " + template.Name;
                string right = Money(template.GetPrice(DetailsCatalog.StandardBrand)) + " (" + quantity + " шт.)";

                Row(TwoColumns(left, right), ConsoleColor.Gray);
            }

            Middle();
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        private void DrawBrandList(PartTemplate template, decimal money)
        {
            Clear();

            Top();
            Row(CategoryName(template.Category) + ": " + template.Name, ConsoleColor.Cyan);
            Middle();
            Row(TwoColumns("производитель", "цена / работа"), ConsoleColor.DarkGray);

            for (int i = 0; i < DetailsCatalog.Brands.Count; i++)
            {
                Brand brand = DetailsCatalog.Brands[i];

                string left = (i + FirstItemNumber) + ". " + brand.Name;
                string right = Money(template.GetPrice(brand)) + " / " + Money(template.GetWork(brand));

                Row(TwoColumns(left, right), ConsoleColor.Gray);
            }

            Middle();
            Row(TwoColumns("в кассе:", Money(money)), money >= 0m ? ConsoleColor.White : ConsoleColor.Red);
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();
        }

        private bool ConfirmBrand(PartTemplate template, Brand brand, decimal money)
        {
            decimal price = template.GetPrice(brand);
            decimal work = template.GetWork(brand);

            Clear();

            Top();
            Row("ПОКУПКА", ConsoleColor.Cyan);
            Row(template.Name + " (" + brand.Name + ")", ConsoleColor.White);
            Middle();
            WriteWrapped(template.Description, 2, ConsoleColor.Gray);
            Middle();
            Row(TwoColumns("цена:", Money(price)), ConsoleColor.White);
            Row(TwoColumns("работа:", Money(work)), ConsoleColor.Gray);
            Row(TwoColumns("выплата за замену:", Money(price + work)), ConsoleColor.Green);
            Row(TwoColumns("в кассе:", Money(money)), money >= price ? ConsoleColor.Gray : ConsoleColor.Red);
            Middle();
            Row(ConfirmCommand + " - купить", ConsoleColor.Gray);
            Row(ExitCommand + " - назад", ConsoleColor.DarkGray);
            Bottom();

            return AskYesNo();
        }

        private bool AskYesNo()
        {
            bool isAnswered = false;
            bool isPositive = false;

            while (isAnswered == false)
            {
                int input = ReadNumber("Решение");

                if (input == ConfirmCommand)
                {
                    isPositive = true;
                    isAnswered = true;
                }
                else if (input == ExitCommand)
                {
                    isPositive = false;
                    isAnswered = true;
                }
                else
                {
                    ShowError("Введите 1 или 0.");
                }
            }

            return isPositive;
        }

        private static string CategoryName(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Body:
                    return "кузов";

                case PartCategory.Electric:
                    return "электрика";

                case PartCategory.Fluid:
                    return "жидкости";

                default:
                    return "двигатель";
            }
        }

        private void DrawNextDayScreen(int day, decimal money)
        {
            Clear();

            Top();
            Row("ДЕНЬ " + day + " ЗАВЕРШЁН", ConsoleColor.Cyan);
            Row(TwoColumns("В кассе:", Money(money)),
                money >= 0m ? ConsoleColor.White : ConsoleColor.Red);
            Middle();
            Row(ConfirmCommand + " - следующая смена", ConsoleColor.Gray);
            Row(ExitCommand + " - закончить игру", ConsoleColor.DarkGray);
            Bottom();
        }

        private static string GetFinalTitle(GameEndReason reason)
        {
            switch (reason)
            {
                case GameEndReason.GoalReached:
                    return "ДОМ КУПЛЕН";

                case GameEndReason.Debt:
                    return "СЕРВИС РАЗОРЁН";

                default:
                    return "СМЕНА ОКОНЧЕНА";
            }
        }

        private static string GetFinalMessage(GameEndReason reason)
        {
            switch (reason)
            {
                case GameEndReason.GoalReached:
                    return "цель достигнута — дом ваш";

                case GameEndReason.Debt:
                    return "касса ушла в минус";

                default:
                    return "игрок завершил смену";
            }
        }

        private static ConsoleColor GetFinalColor(GameEndReason reason)
        {
            switch (reason)
            {
                case GameEndReason.GoalReached:
                    return ConsoleColor.Green;

                case GameEndReason.Debt:
                    return ConsoleColor.Red;

                default:
                    return ConsoleColor.Cyan;
            }
        }

        private static void Clear()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
            }
        }

        private static int ReadNumber(string label)
        {
            int number = 0;
            bool isNumber = false;

            while (isNumber == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("  " + label + " > ");
                Console.ResetColor();

                string text = Console.ReadLine();

                if (text == null)
                {
                    throw new InvalidOperationException("Ввод закончился: поток ввода закрыт.");
                }

                isNumber = int.TryParse(text, out number) && number >= 0;

                if (isNumber == false)
                {
                    ShowError("Нужно целое число от 0 и больше.");
                }
            }

            return number;
        }

        private static void ShowError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  " + text);
            Console.ResetColor();
        }

        private static string Money(decimal value)
        {
            string number = Math.Abs(value).ToString("N0", CultureInfo.InvariantCulture).Replace(",", " ");
            string sign = value < 0m ? "-" : string.Empty;

            return sign + number + " " + Currency;
        }

        private static string DetailName(Detail detail) =>
            detail == null ? "неизвестная деталь" : detail.Name;

        private static string BrandName(Detail detail) =>
            detail == null ? "без производителя" : detail.BrandName;

        private static string TwoColumns(string left, string right)
        {
            int gap = TextWidth - left.Length - right.Length;

            if (gap < 1)
            {
                gap = 1;
            }

            return left + new string(' ', gap) + right;
        }

        private static string Center(string text) =>
            new string(' ', Math.Max(0, (TextWidth - text.Length) / 2)) + text;

        private static void Top() => BorderLine('┌', '─', '┐');

        private static void Middle() => BorderLine('├', '─', '┤');

        private static void Bottom() => BorderLine('└', '─', '┘');

        private static void BorderLine(char left, char fill, char right)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(left + new string(fill, TextWidth + 2) + right);
            Console.ResetColor();
        }

        private static void DoubleLine(char left, char fill, char right)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(left + new string(fill, TextWidth + 2) + right);
            Console.ResetColor();
        }

        private static void Row(string text) => Row(text, ConsoleColor.Gray);

        private static void Row(string text, ConsoleColor color)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            if (text.Length > TextWidth)
            {
                text = text.Substring(0, TextWidth);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│");
            Console.ForegroundColor = color;
            Console.Write(" " + text.PadRight(TextWidth) + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("│");
            Console.ResetColor();
        }

        private static void DoubleRow(string text, ConsoleColor color)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            if (text.Length > TextWidth)
            {
                text = text.Substring(0, TextWidth);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("║");
            Console.ForegroundColor = color;
            Console.Write(" " + text.PadRight(TextWidth) + " ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║");
            Console.ResetColor();
        }

        private static void WriteWrapped(string text, int indent, ConsoleColor color)
        {
            string padding = new string(' ', indent);
            int width = TextWidth - indent;

            foreach (string line in Wrap(text, width))
            {
                Row(padding + line, color);
            }
        }

        private static List<string> Wrap(string text, int width)
        {
            var lines = new List<string>();

            if (string.IsNullOrEmpty(text))
            {
                lines.Add(string.Empty);

                return lines;
            }

            var current = new StringBuilder();

            foreach (string word in text.Split(' '))
            {
                if (word.Length == 0)
                {
                    continue;
                }

                if (current.Length == 0)
                {
                    current.Append(word);
                }
                else if (current.Length + 1 + word.Length <= width)
                {
                    current.Append(' ');
                    current.Append(word);
                }
                else
                {
                    lines.Add(current.ToString());
                    current.Clear();
                    current.Append(word);
                }
            }

            if (current.Length > 0)
            {
                lines.Add(current.ToString());
            }

            return lines;
        }
    }
}