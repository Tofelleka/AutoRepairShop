using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AutoRepairShop.UI
{
    internal enum DayCommand
    {
        EndDay = 0,
        TakeCar = 1,
        Shop = 2
    }

    internal class ShopItem
    {
        internal ShopItem(Detail detail, int quantity)
        {
            Detail = detail;
            Quantity = quantity;
        }

        internal Detail Detail { get; }
        internal int Quantity { get; }
    }

    internal class Illustrator
    {
        private const int TextWidth = 58;
        private const string Currency = "руб.";

        internal Illustrator()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch (System.IO.IOException)
            {
            }
        }

        internal void ShowBanner(decimal startMoney)
        {
            Clear();

            DoubleLine('╔', '═', '╗');
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("АВТОСЕРВИС"), ConsoleColor.Cyan);
            DoubleRow(Center("«ГАРАЖ 2108»"), ConsoleColor.Cyan);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("стартовый капитал: " + Money(startMoney)), ConsoleColor.White);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleLine('╚', '═', '╝');

            Pause();
        }

        internal DayCommand AskDayCommand(int day, decimal money, int carsInQueue)
        {
            Clear();

            Top();
            Row("ДЕНЬ " + day, ConsoleColor.Cyan);
            Row(TwoColumns("В кассе:", Money(money)), ConsoleColor.White);
            Row(TwoColumns("В очереди машин:", carsInQueue + " шт."),
                carsInQueue > 0 ? ConsoleColor.Green : ConsoleColor.DarkGray);
            Middle();
            Row("1 - принять машину", carsInQueue > 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray);
            Row("2 - магазин запчастей", ConsoleColor.Gray);
            Row("0 - закончить день", ConsoleColor.DarkGray);
            Bottom();

            while (true)
            {
                int input = ReadNumber("Команда");

                switch (input)
                {
                    case 0:
                        return DayCommand.EndDay;

                    case 1:
                        return DayCommand.TakeCar;

                    case 2:
                        return DayCommand.Shop;

                    default:
                        ShowError("Такой команды нет. Введите 1, 2 или 0.");
                        break;
                }
            }
        }

        internal void ShowNoCars()
        {
            Clear();

            Top();
            Row("Очередь пуста.", ConsoleColor.DarkGray);
            Row("Новые машины приедут на следующий день.", ConsoleColor.DarkGray);
            Bottom();
        }

        internal bool AskRepairCar(Car car)
        {
            Clear();

            Top();
            Row("МАШИНА НА ПОДЪЁМНИКЕ", ConsoleColor.Cyan);
            Row(car.Name, ConsoleColor.White);
            Middle();
            Row("Ремонтируем?", ConsoleColor.Gray);
            Row("1 - да, берём в работу", ConsoleColor.Gray);
            Row("0 - отказать (штраф)", ConsoleColor.DarkGray);
            Bottom();

            while (true)
            {
                int input = ReadNumber("Решение");

                if (input == 1)
                {
                    return true;
                }

                if (input == 0)
                {
                    return false;
                }

                ShowError("Введите 1 или 0.");
            }
        }

        internal DetailType? AskDetailToRepair(IReadOnlyList<Detail> brokenDetails)
        {
            Clear();

            Top();
            Row("ДИАГНОСТИКА", ConsoleColor.Cyan);

            if (brokenDetails == null || brokenDetails.Count == 0)
            {
                Row("Сломанных деталей нет.", ConsoleColor.Green);
                Bottom();

                return null;
            }

            Row(TwoColumns("деталь", "цена + работа"), ConsoleColor.DarkGray);
            Middle();

            for (int i = 0; i < brokenDetails.Count; i++)
            {
                Detail detail = brokenDetails[i];

                string left = (i + 1) + ". " + detail.Name;
                string right = Money(detail.Price + detail.ReplacementCost);

                Row(TwoColumns(left, right), ConsoleColor.Gray);
            }

            Middle();
            Row("0 - прекратить ремонт", ConsoleColor.DarkGray);
            Row("(штраф за каждую оставшуюся сломанной деталь)", ConsoleColor.DarkGray);
            Bottom();

            while (true)
            {
                int input = ReadNumber("Деталь");

                if (input == 0)
                {
                    return null;
                }

                if (input >= 1 && input <= brokenDetails.Count)
                {
                    return brokenDetails[input - 1].Type;
                }

                ShowError("Такой позиции нет в списке.");
            }
        }

        internal void ShowDetailRepair(DetailRepairReport report)
        {
            Top();

            switch (report.Outcome)
            {
                case RepairOutcome.ReplacedBroken:
                    Row("[+] " + DetailName(report.Detail) + " заменена", ConsoleColor.Green);
                    Row("    начислено: " + Money(report.Charge), ConsoleColor.Green);
                    break;

                case RepairOutcome.ReplacedHealthy:
                    Row("[!] " + DetailName(report.Detail) + " была исправна", ConsoleColor.Yellow);
                    Row("    новая деталь списана со склада без оплаты", ConsoleColor.Yellow);
                    break;

                case RepairOutcome.NoDetailInStock:
                    Row("[-] На складе нет детали: " + DetailName(report.Detail), ConsoleColor.Red);
                    break;

                default:
                    Row("[!] Неизвестный исход ремонта", ConsoleColor.Yellow);
                    break;
            }

            Bottom();
        }

        internal void ShowRepairReport(RepairReport report, decimal money)
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

        internal DetailType? AskPartToBuy(IReadOnlyList<ShopItem> items, decimal money, int totalQuantity, int capacity)
        {
            Clear();

            Top();
            Row("МАГАЗИН ЗАПЧАСТЕЙ", ConsoleColor.Cyan);
            Row(TwoColumns("В кассе:", Money(money)), ConsoleColor.White);
            Row(TwoColumns("Склад:", totalQuantity + " / " + capacity), ConsoleColor.Gray);
            Middle();
            Row(TwoColumns("деталь", "цена (на складе)"), ConsoleColor.DarkGray);

            for (int i = 0; i < items.Count; i++)
            {
                ShopItem item = items[i];

                string left = (i + 1) + ". " + item.Detail.Name;
                string right = Money(item.Detail.Price) + " (" + item.Quantity + " шт.)";

                Row(TwoColumns(left, right), ConsoleColor.Gray);
            }

            Middle();
            Row("0 - выйти из магазина", ConsoleColor.DarkGray);
            Bottom();

            while (true)
            {
                int input = ReadNumber("Деталь");

                if (input == 0)
                {
                    return null;
                }

                if (input >= 1 && input <= items.Count)
                {
                    return items[input - 1].Detail.Type;
                }

                ShowError("Такой позиции нет в списке.");
            }
        }

        internal void ShowPurchase(PurchaseReport report, decimal money)
        {
            Top();

            switch (report.Outcome)
            {
                case PurchaseOutcome.Bought:
                    Row("[+] Куплено: " + DetailName(report.Detail), ConsoleColor.Green);
                    Row("    потрачено: " + Money(report.Detail.Price), ConsoleColor.Green);
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

        internal bool AskNextDay(int day, decimal money)
        {
            Clear();

            Top();
            Row("ДЕНЬ " + day + " ЗАВЕРШЁН", ConsoleColor.Cyan);
            Row(TwoColumns("В кассе:", Money(money)),
                money >= 0m ? ConsoleColor.White : ConsoleColor.Red);
            Middle();
            Row("1 - следующая смена", ConsoleColor.Gray);
            Row("0 - закончить игру", ConsoleColor.DarkGray);
            Bottom();

            while (true)
            {
                int input = ReadNumber("Решение");

                if (input == 1)
                {
                    return true;
                }

                if (input == 0)
                {
                    return false;
                }

                ShowError("Введите 1 или 0.");
            }
        }

        internal void ShowFinal(decimal money, int days)
        {
            Clear();

            DoubleLine('╔', '═', '╗');
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("СМЕНА ОКОНЧЕНА"), ConsoleColor.Cyan);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleRow(Center("дней отработано: " + days), ConsoleColor.White);
            DoubleRow(Center("в кассе: " + Money(money)),
                money >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            DoubleRow(Center(money >= 0m ? "смена в плюсе" : "смена в минусе"),
                money >= 0m ? ConsoleColor.Green : ConsoleColor.Red);
            DoubleRow(string.Empty, ConsoleColor.Gray);
            DoubleLine('╚', '═', '╝');

            Pause();
        }

        internal void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Нажмите Enter, чтобы продолжить...");
            Console.ResetColor();
            Console.ReadLine();
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
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("  " + label + " > ");
                Console.ResetColor();

                string text = Console.ReadLine();
                int number;

                if (int.TryParse(text, out number) && number >= 0)
                {
                    return number;
                }

                ShowError("Нужно целое число от 0 и больше.");
            }
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

        private static string DetailName(Detail detail)
        {
            return detail == null ? "неизвестная деталь" : detail.Name;
        }

        private static string TwoColumns(string left, string right)
        {
            int gap = TextWidth - left.Length - right.Length;

            if (gap < 1)
            {
                gap = 1;
            }

            return left + new string(' ', gap) + right;
        }

        private static string Center(string text)
        {
            int pad = (TextWidth - text.Length) / 2;

            if (pad < 0)
            {
                pad = 0;
            }

            return new string(' ', pad) + text;
        }

        private static void Top()
        {
            BorderLine('┌', '─', '┐');
        }

        private static void Middle()
        {
            BorderLine('├', '─', '┤');
        }

        private static void Bottom()
        {
            BorderLine('└', '─', '┘');
        }

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

        private static void Row(string text)
        {
            Row(text, ConsoleColor.Gray);
        }

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
    }
}
