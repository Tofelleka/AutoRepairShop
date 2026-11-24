using System;

namespace AutoRepairShop
{
    internal class SystemRandomSource : IRandomSource
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);

        public int NextInclusive(int minValue, int maxValue) => _random.Next(minValue, maxValue + 1);

        public bool NextChance(double chance) => _random.NextDouble() < chance;
    }
}