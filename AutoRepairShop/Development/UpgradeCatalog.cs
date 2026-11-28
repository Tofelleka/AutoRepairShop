using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class UpgradeCatalog
    {
        private const int Levels = 3;

        private static readonly List<UpgradeInfo> Upgrades = CreateUpgrades();

        public static IReadOnlyList<UpgradeInfo> All => Upgrades;

        public static UpgradeInfo Get(UpgradeType type)
        {
            foreach (UpgradeInfo info in Upgrades)
            {
                if (info.Type == type)
                {
                    return info;
                }
            }

            throw new ArgumentException($"Неизвестный тип улучшения: {type}.", nameof(type));
        }

        private static List<UpgradeInfo> CreateUpgrades()
        {
            return new List<UpgradeInfo>
            {
                new UpgradeInfo(UpgradeType.Warehouse, "Склад", "+15 мест", Levels, 15,
                    new[] { 150_000m, 300_000m, 600_000m }),
                new UpgradeInfo(UpgradeType.Marketing, "Реклама", "+1 машина в день", Levels, 1,
                    new[] { 200_000m, 400_000m, 800_000m }),
                new UpgradeInfo(UpgradeType.Skill, "Квалификация", "+10% к работе", Levels, 10,
                    new[] { 100_000m, 200_000m, 400_000m }),
                new UpgradeInfo(UpgradeType.BodyBox, "Кузовной бокс", "+10% к кузовным работам", Levels, 10,
                    new[] { 400_000m, 650_000m, 1_000_000m }),
                new UpgradeInfo(UpgradeType.ElectricBox, "Бокс электрика", "+10% к электрике", Levels, 10,
                    new[] { 350_000m, 550_000m, 850_000m }),
                new UpgradeInfo(UpgradeType.TuningBox, "Тюнинг-бокс", "уровень тюнинга", Levels, 1,
                    new[] { 300_000m, 500_000m, 800_000m })
            };
        }
    }
}
