using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace AutoRepairShop
{
    internal class GameSave
    {
        private const string Version = "2";

        public GameSave(
            decimal money,
            int day,
            IReadOnlyDictionary<UpgradeType, int> levels,
            IReadOnlyDictionary<DetailType, int> stock,
            ShiftStatistics shift,
            int bodyBusyUntilDay,
            IReadOnlyCollection<CourseType> courses,
            decimal earned)
        {
            Money = money;
            Day = day;
            Levels = levels;
            Stock = stock;
            Shift = shift;
            BodyBusyUntilDay = bodyBusyUntilDay;
            Courses = courses;
            Earned = earned;
        }

        public decimal Money { get; }

        public int Day { get; }

        public IReadOnlyDictionary<UpgradeType, int> Levels { get; }

        public IReadOnlyDictionary<DetailType, int> Stock { get; }

        public ShiftStatistics Shift { get; }

        public int BodyBusyUntilDay { get; }

        public IReadOnlyCollection<CourseType> Courses { get; }

        public decimal Earned { get; }

        public static string GetPath(int slot)
        {
            if (slot < 1 || slot > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "Номер слота должен быть от 1 до 3.");
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save" + slot + ".txt");
        }

        public static bool Exists(string path) => File.Exists(path);

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void Write(string path, GameSave save)
        {
            var builder = new StringBuilder();

            builder.AppendLine("version=" + Version);
            builder.AppendLine("money=" + save.Money.ToString(CultureInfo.InvariantCulture));
            builder.AppendLine("day=" + save.Day);
            builder.AppendLine("levels=" + JoinLevels(save.Levels));
            builder.AppendLine("stock=" + JoinStock(save.Stock));
            builder.AppendLine("shift=" + JoinShift(save.Shift));
            builder.AppendLine("bodybusy=" + save.BodyBusyUntilDay);
            builder.AppendLine("courses=" + JoinCourses(save.Courses));
            builder.AppendLine("earned=" + save.Earned.ToString(CultureInfo.InvariantCulture));

            string tempPath = path + ".tmp";

            File.WriteAllText(tempPath, builder.ToString(), Encoding.UTF8);

            if (File.Exists(path))
            {
                File.Replace(tempPath, path, null);
            }
            else
            {
                File.Move(tempPath, path);
            }
        }

        public static bool TryRead(string path, out GameSave save)
        {
            save = null;

            if (File.Exists(path) == false)
            {
                return false;
            }

            try
            {
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);

                decimal money = 0m;
                int day = 1;
                int bodyBusyUntilDay = 0;
                decimal earned = 0m;
                var courses = new List<CourseType>();
                bool hasKnownField = false;
                var levels = new Dictionary<UpgradeType, int>();
                var stock = new Dictionary<DetailType, int>();
                ShiftStatistics shift = CreateEmptyShift();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    int index = line.IndexOf('=');

                    if (index < 0)
                    {
                        continue;
                    }

                    string key = line.Substring(0, index);
                    string value = line.Substring(index + 1);

                    switch (key)
                    {
                        case "money":
                            money = decimal.Parse(value, CultureInfo.InvariantCulture);
                            hasKnownField = true;
                            break;

                        case "day":
                            day = int.Parse(value, CultureInfo.InvariantCulture);
                            hasKnownField = true;
                            break;

                        case "levels":
                            ParseLevels(value, levels);
                            hasKnownField = true;
                            break;

                        case "stock":
                            ParseStock(value, stock);
                            hasKnownField = true;
                            break;

                        case "shift":
                            shift = ParseShift(value);
                            hasKnownField = true;
                            break;

                        case "bodybusy":
                            bodyBusyUntilDay = int.Parse(value, CultureInfo.InvariantCulture);
                            hasKnownField = true;
                            break;

                        case "courses":
                            ParseCourses(value, courses);
                            hasKnownField = true;
                            break;

                        case "earned":
                            earned = decimal.Parse(value, CultureInfo.InvariantCulture);
                            hasKnownField = true;
                            break;
                    }
                }

                if (hasKnownField == false)
                {
                    save = null;

                    return false;
                }

                save = new GameSave(money, day, levels, stock, shift, bodyBusyUntilDay, courses, earned);

                return true;
            }
            catch (Exception)
            {
                save = null;

                return false;
            }
        }

        public static ShiftStatistics CreateEmptyShift() =>
            new ShiftStatistics(0, 0, 0, 0, 0m, 0m, 0m, 0m, 0m);

        private static string JoinLevels(IReadOnlyDictionary<UpgradeType, int> levels)
        {
            var parts = new List<string>();

            foreach (var pair in levels)
            {
                parts.Add(pair.Key + ":" + pair.Value);
            }

            return string.Join(",", parts);
        }

        private static string JoinStock(IReadOnlyDictionary<DetailType, int> stock)
        {
            var parts = new List<string>();

            foreach (var pair in stock)
            {
                parts.Add(pair.Key + ":" + pair.Value);
            }

            return string.Join(",", parts);
        }

        private static string JoinCourses(IReadOnlyCollection<CourseType> courses)
        {
            var parts = new List<string>();

            foreach (CourseType type in courses)
            {
                parts.Add(type.ToString());
            }

            return string.Join(",", parts);
        }

        private static void ParseCourses(string text, List<CourseType> courses)
        {
            foreach (string item in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse(item, out CourseType type))
                {
                    courses.Add(type);
                }
            }
        }

        private static string JoinShift(ShiftStatistics shift)
        {
            var parts = new List<string>
            {
                shift.Days.ToString(CultureInfo.InvariantCulture),
                shift.Cars.ToString(CultureInfo.InvariantCulture),
                shift.Repaired.ToString(CultureInfo.InvariantCulture),
                shift.Refused.ToString(CultureInfo.InvariantCulture),
                shift.Revenue.ToString(CultureInfo.InvariantCulture),
                shift.Fines.ToString(CultureInfo.InvariantCulture),
                shift.Rent.ToString(CultureInfo.InvariantCulture),
                shift.PurchaseCosts.ToString(CultureInfo.InvariantCulture),
                shift.UpgradeCosts.ToString(CultureInfo.InvariantCulture)
            };

            return string.Join(";", parts);
        }

        private static ShiftStatistics ParseShift(string text)
        {
            string[] parts = text.Split(';');

            if (parts.Length != 9)
            {
                throw new FormatException("Некорректная статистика смены в сохранении.");
            }

            return new ShiftStatistics(
                int.Parse(parts[0], CultureInfo.InvariantCulture),
                int.Parse(parts[1], CultureInfo.InvariantCulture),
                int.Parse(parts[2], CultureInfo.InvariantCulture),
                int.Parse(parts[3], CultureInfo.InvariantCulture),
                decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                decimal.Parse(parts[5], CultureInfo.InvariantCulture),
                decimal.Parse(parts[6], CultureInfo.InvariantCulture),
                decimal.Parse(parts[7], CultureInfo.InvariantCulture),
                decimal.Parse(parts[8], CultureInfo.InvariantCulture));
        }

        private static void ParseLevels(string text, Dictionary<UpgradeType, int> levels)
        {
            foreach (string pair in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = pair.Split(':');

                if (parts.Length != 2)
                {
                    continue;
                }

                if (Enum.TryParse(parts[0], out UpgradeType type))
                {
                    levels[type] = int.Parse(parts[1], CultureInfo.InvariantCulture);
                }
            }
        }

        private static void ParseStock(string text, Dictionary<DetailType, int> stock)
        {
            foreach (string pair in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = pair.Split(':');

                if (parts.Length != 2)
                {
                    continue;
                }

                if (Enum.TryParse(parts[0], out DetailType type))
                {
                    stock[type] = int.Parse(parts[1], CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
