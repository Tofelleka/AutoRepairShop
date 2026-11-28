using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace AutoRepairShop
{
    internal class Profile
    {
        private const string FileName = "profile.txt";

        private readonly Achievements _achievements;

        public Profile(decimal bestMoney, int bestDays, IEnumerable<AchievementType> achievements, int successfulRepaints = 0)
        {
            BestMoney = bestMoney;
            BestDays = bestDays;
            SuccessfulRepaints = successfulRepaints;
            _achievements = new Achievements(achievements);
        }

        public decimal BestMoney { get; private set; }

        public int BestDays { get; private set; }

        public int SuccessfulRepaints { get; private set; }

        public IReadOnlyCollection<AchievementType> Achievements => _achievements.All;

        public bool IsUnlocked(AchievementType type) => _achievements.IsUnlocked(type);

        public static string GetPath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

        public static Profile Load() => Load(GetPath());

        public static Profile Load(string path)
        {
            if (File.Exists(path) == false)
            {
                return new Profile(0m, 0, new List<AchievementType>(), 0);
            }

            try
            {
                decimal bestMoney = 0m;
                int bestDays = 0;
                int repaints = 0;
                var achievements = new List<AchievementType>();

                foreach (string line in File.ReadAllLines(path, Encoding.UTF8))
                {
                    int index = line.IndexOf('=');

                    if (index < 0)
                    {
                        continue;
                    }

                    string key = line.Substring(0, index);
                    string value = line.Substring(index + 1);

                    switch (key)
                    {
                        case "bestmoney":
                            bestMoney = decimal.Parse(value, CultureInfo.InvariantCulture);
                            break;

                        case "bestdays":
                            bestDays = int.Parse(value, CultureInfo.InvariantCulture);
                            break;

                        case "repaints":
                            repaints = int.Parse(value, CultureInfo.InvariantCulture);
                            break;

                        case "achievements":
                            ParseAchievements(value, achievements);
                            break;
                    }
                }

                return new Profile(bestMoney, bestDays, achievements, repaints);
            }
            catch (Exception)
            {
                return new Profile(0m, 0, new List<AchievementType>(), 0);
            }
        }

        public bool BeatsRecord(decimal money) => money > BestMoney;

        public void UpdateRecord(decimal money, int days)
        {
            if (money <= BestMoney)
            {
                return;
            }

            BestMoney = money;
            BestDays = days;
        }

        public bool Unlock(AchievementType type) => _achievements.Unlock(type);

        public void IncrementRepaints() => SuccessfulRepaints++;

        public void Save() => Save(GetPath());

        public void Save(string path)
        {
            try
            {
                var builder = new StringBuilder();
                builder.AppendLine("bestmoney=" + BestMoney.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("bestdays=" + BestDays.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("repaints=" + SuccessfulRepaints.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("achievements=" + JoinAchievements());

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
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private string JoinAchievements()
        {
            var parts = new List<string>();

            foreach (AchievementType type in _achievements.All)
            {
                parts.Add(type.ToString());
            }

            return string.Join(",", parts);
        }

        private static void ParseAchievements(string text, List<AchievementType> achievements)
        {
            foreach (string item in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse(item, out AchievementType type))
                {
                    achievements.Add(type);
                }
            }
        }
    }
}
