using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class ServiceDevelopment
    {
        private readonly Dictionary<UpgradeType, int> _levels = new Dictionary<UpgradeType, int>();

        public ServiceDevelopment() : this(new Dictionary<UpgradeType, int>())
        {
        }

        public ServiceDevelopment(IReadOnlyDictionary<UpgradeType, int> levels)
        {
            if (levels == null)
            {
                throw new ArgumentNullException(nameof(levels));
            }

            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                int level = levels.TryGetValue(type, out int savedLevel) ? savedLevel : 0;

                if (level < 0 || level > UpgradeCatalog.Get(type).MaxLevel)
                {
                    throw new ArgumentOutOfRangeException(nameof(levels), $"Недопустимый уровень улучшения {type}: {level}.");
                }

                _levels[type] = level;
            }
        }

        public int GetLevel(UpgradeType type)
        {
            if (_levels.TryGetValue(type, out int level) == false)
            {
                throw new ArgumentException($"Неизвестный тип улучшения: {type}.", nameof(type));
            }

            return level;
        }

        public bool CanUpgrade(UpgradeType type) => UpgradeCatalog.Get(type).HasNextLevel(GetLevel(type));

        public decimal GetUpgradePrice(UpgradeType type) => UpgradeCatalog.Get(type).GetPrice(GetLevel(type));

        public void ApplyUpgrade(UpgradeType type)
        {
            if (CanUpgrade(type) == false)
            {
                throw new InvalidOperationException($"Улучшение {type} уже максимального уровня.");
            }

            _levels[type] = GetLevel(type) + 1;
        }
    }
}
