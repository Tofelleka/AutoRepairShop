using System;

namespace AutoRepairShop
{
    internal static class Utils
    {
        public const int FirstIndex = 0;

        private static Random _random = new Random();

        public static int ReadNumber()
        {
            bool isWork = true;

            int numberForReturn = 0;

            while (isWork)
            {
                if (int.TryParse(Console.ReadLine(), out int number) && number >= 0)
                {
                    isWork = false;
                    numberForReturn = number;
                }
                else
                {
                    Console.WriteLine("\n\nНеверный ввод, это должно быть положительное число или 0.");
                }
            }

            return numberForReturn;
        }

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber);
        }

        public static bool GetRandomBool(double chance)
        {
            return _random.NextDouble() <= chance;
        }

        public static bool ContainsDetailType(int userInput)
        {
            if (Enum.IsDefined(typeof(DetailType), userInput))
            {
                return true;
            }

            return false;
        }
    }
}