using System;

namespace AutoRepairShop
{
    internal class UpgradeInfo
    {
        private readonly decimal[] _prices;

        public UpgradeInfo(
            UpgradeType type,
            string name,
            string effectDescription,
            int maxLevel,
            int effectPerLevel,
            decimal[] prices)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название улучшения не может быть пустым.", nameof(name));
            }

            if (maxLevel <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLevel), "Максимальный уровень должен быть положительным.");
            }

            if (effectPerLevel <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(effectPerLevel), "Эффект за уровень должен быть положительным.");
            }

            if (prices == null)
            {
                throw new ArgumentNullException(nameof(prices));
            }

            if (prices.Length != maxLevel)
            {
                throw new ArgumentException("Количество цен должно совпадать с числом уровней.", nameof(prices));
            }

            Type = type;
            Name = name;
            EffectDescription = effectDescription;
            MaxLevel = maxLevel;
            EffectPerLevel = effectPerLevel;
            _prices = prices;
        }

        public UpgradeType Type { get; }

        public string Name { get; }

        public string EffectDescription { get; }

        public int MaxLevel { get; }

        public int EffectPerLevel { get; }

        public bool HasNextLevel(int level) => level < MaxLevel;

        public decimal GetPrice(int level)
        {
            if (HasNextLevel(level) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Улучшение уже максимального уровня.");
            }

            return _prices[level];
        }
    }
}
