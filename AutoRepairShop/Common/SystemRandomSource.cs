using System;

namespace AutoRepairShop.Common
{
    internal class SystemRandomSource : IRandomSource
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public int NextInclusive(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        public bool NextChance(double chance)
        {
            return _random.NextDouble() < chance;
        }
    }
}