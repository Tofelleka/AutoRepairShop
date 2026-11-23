using System;

namespace AutoRepairShop
{
    internal static class Utils
    {
        public const int FirstIndex = 0;

        private static Random _random = new Random();

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber);
        }

        public static bool GetRandomBool(double chance)
        {
            return _random.NextDouble() <= chance;
        }
    }
}