using System;

namespace AutoRepairShop
{
    internal static class WarehouseUpgrade
    {
        public const int BaseCapacity = 60;
        public const int CapacityPerLevel = 15;
        public const int MaxLevel = 3;

        private static readonly Decimal[] Prices = { 5000m, 10000m, 20000m };

        public static bool HasNextLevel(int level) => level < MaxLevel;

        public static decimal GetPrice(int level)
        {
            if(HasNextLevel(level) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Достигнут максимальный уровень склада.");
            }

            return Prices[level];
        }
    }
}   