using System;

namespace AutoRepairShop.UI
{
    internal class SystemRandomSource : IRandomSource
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public bool NextChance(double chance)
        {
            return _random.NextDouble() < chance;
        }
    }
}