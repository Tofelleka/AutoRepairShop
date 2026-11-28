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
            decimal earned,
            IReadOnlyList<DeliveryOrder> deliveries,
            IReadOnlyList<Car> queueCars,
            Car postponedCar,
            IReadOnlyDictionary<int, int> ownerScores,
            IReadOnlyDictionary<int, int> ownerFlags,
            IReadOnlyDictionary<int, int> ownerReferrers,
            Car currentCar)
        {
            Money = money;
            Day = day;
            Levels = levels;
            Stock = stock;
            Shift = shift;
            BodyBusyUntilDay = bodyBusyUntilDay;
            Courses = courses;
            Earned = earned;
            Deliveries = deliveries;
            QueueCars = queueCars;
            PostponedCar = postponedCar;
            OwnerScores = ownerScores;
            OwnerFlags = ownerFlags;
            OwnerReferrers = ownerReferrers;
            CurrentCar = currentCar;
        }

        public decimal Money { get; }

        public int Day { get; }

        public IReadOnlyDictionary<UpgradeType, int> Levels { get; }

        public IReadOnlyDictionary<DetailType, int> Stock { get; }

        public ShiftStatistics Shift { get; }

        public int BodyBusyUntilDay { get; }

        public IReadOnlyCollection<CourseType> Courses { get; }

        public decimal Earned { get; }

        public IReadOnlyList<DeliveryOrder> Deliveries { get; }

        public IReadOnlyList<Car> QueueCars { get; }

        public Car PostponedCar { get; }

        public IReadOnlyDictionary<int, int> OwnerScores { get; }

        public IReadOnlyDictionary<int, int> OwnerFlags { get; }

        public IReadOnlyDictionary<int, int> OwnerReferrers { get; }

        public Car CurrentCar { get; }

        public static string RootDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public static string GetPath(int slot)
        {
            if (slot < 1 || slot > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "Номер слота должен быть от 1 до 3.");
            }

            return Path.Combine(RootDirectory, "save" + slot + ".txt");
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
            builder.AppendLine("deliveries=" + JoinDeliveries(save.Deliveries));
            builder.AppendLine("queue=" + JoinCars(save.QueueCars));
            builder.AppendLine("postponed=" + (save.PostponedCar == null ? string.Empty : CarSerializer.Serialize(save.PostponedCar)));
            builder.AppendLine("owners=" + JoinOwners(save.OwnerScores, save.OwnerFlags, save.OwnerReferrers));
            builder.AppendLine("current=" + (save.CurrentCar == null ? string.Empty : CarSerializer.Serialize(save.CurrentCar)));

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
                var deliveries = new List<DeliveryOrder>();
                var queueCars = new List<Car>();
                var ownerScores = new Dictionary<int, int>();
                var ownerFlags = new Dictionary<int, int>();
                var ownerReferrers = new Dictionary<int, int>();
                Car postponedCar = null;
                Car currentCar = null;
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

                        case "deliveries":
                            ParseDeliveries(value, deliveries);
                            hasKnownField = true;
                            break;

                        case "owners":
                            ParseOwners(value, ownerScores, ownerFlags, ownerReferrers);
                            hasKnownField = true;
                            break;

                        case "current":
                            if (string.IsNullOrEmpty(value) == false)
                            {
                                currentCar = CarSerializer.Deserialize(value);
                            }

                            hasKnownField = true;
                            break;

                        case "queue":
                            ParseCars(value, queueCars);
                            hasKnownField = true;
                            break;

                        case "postponed":
                            if (string.IsNullOrEmpty(value) == false)
                            {
                                postponedCar = CarSerializer.Deserialize(value);
                            }

                            hasKnownField = true;
                            break;
                    }
                }

                if (hasKnownField == false)
                {
                    save = null;

                    return false;
                }

                save = new GameSave(
                    money,
                    day,
                    levels,
                    stock,
                    shift,
                    bodyBusyUntilDay,
                    courses,
                    earned,
                    deliveries,
                    queueCars,
                    postponedCar,
                    ownerScores,
                    ownerFlags,
                    ownerReferrers,
                    currentCar);

                return true;
            }
            catch (Exception)
            {
                save = null;

                return false;
            }
        }

        public static ShiftStatistics CreateEmptyShift() =>
            new ShiftStatistics(0, 0, 0, 0, 0m, 0m, 0m, 0m, 0m, 0);

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

        private static string JoinOwners(
            IReadOnlyDictionary<int, int> scores,
            IReadOnlyDictionary<int, int> flags,
            IReadOnlyDictionary<int, int> referrers)
        {
            var parts = new List<string>();

            foreach (KeyValuePair<int, int> pair in scores)
            {
                int flagValue = flags.TryGetValue(pair.Key, out int stored) ? stored : 2;
                int referrer = referrers.TryGetValue(pair.Key, out int storedReferrer) ? storedReferrer : -1;

                parts.Add(pair.Key.ToString(CultureInfo.InvariantCulture) + ":" +
                    pair.Value.ToString(CultureInfo.InvariantCulture) + ":" +
                    flagValue.ToString(CultureInfo.InvariantCulture) + ":" +
                    referrer.ToString(CultureInfo.InvariantCulture));
            }

            return string.Join(",", parts);
        }

        private static void ParseOwners(
            string text,
            Dictionary<int, int> scores,
            Dictionary<int, int> flags,
            Dictionary<int, int> referrers)
        {
            foreach (string item in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = item.Split(':');

                if (parts.Length < 2)
                {
                    continue;
                }

                if (int.TryParse(parts[0], out int id) == false || int.TryParse(parts[1], out int value) == false)
                {
                    continue;
                }

                scores[id] = value;
                flags[id] = parts.Length >= 3 && int.TryParse(parts[2], out int flagValue) ? flagValue : 2;
                referrers[id] = parts.Length >= 4 && int.TryParse(parts[3], out int referrerId) ? referrerId : -1;
            }
        }

        private static string JoinCars(IReadOnlyList<Car> cars)
        {
            var parts = new List<string>();

            foreach (Car car in cars)
            {
                parts.Add(CarSerializer.Serialize(car));
            }

            return string.Join("|", parts);
        }

        private static void ParseCars(string text, List<Car> cars)
        {
            foreach (string item in text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    cars.Add(CarSerializer.Deserialize(item));
                }
                catch (FormatException)
                {
                }
            }
        }

        private static string JoinDeliveries(IReadOnlyList<DeliveryOrder> deliveries)
        {
            var parts = new List<string>();

            foreach (DeliveryOrder order in deliveries)
            {
                parts.Add(
                    order.Type + ":" + order.BrandName + ":" + order.ArrivalDay.ToString(CultureInfo.InvariantCulture));
            }

            return string.Join(";", parts);
        }

        private static void ParseDeliveries(string text, List<DeliveryOrder> deliveries)
        {
            foreach (string item in text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = item.Split(':');

                if (parts.Length != 3)
                {
                    continue;
                }

                if (Enum.TryParse(parts[0], out DetailType type) == false)
                {
                    continue;
                }

                deliveries.Add(new DeliveryOrder(
                    type,
                    parts[1],
                    int.Parse(parts[2], CultureInfo.InvariantCulture)));
            }
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
                shift.UpgradeCosts.ToString(CultureInfo.InvariantCulture),
                shift.Referrals.ToString(CultureInfo.InvariantCulture)
            };

            return string.Join(";", parts);
        }

        private static ShiftStatistics ParseShift(string text)
        {
            string[] parts = text.Split(';');

            if (parts.Length != 10)
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
                decimal.Parse(parts[8], CultureInfo.InvariantCulture),
                int.Parse(parts[9], CultureInfo.InvariantCulture));
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
