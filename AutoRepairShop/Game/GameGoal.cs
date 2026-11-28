using System;

namespace AutoRepairShop
{
    internal class GameGoal
    {
        public GameGoal(string title, decimal targetMoney)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Название цели не может быть пустым.", nameof(title));
            }

            if (targetMoney <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(targetMoney), "Цель должна быть положительной суммой.");
            }

            Title = title;
            TargetMoney = targetMoney;
        }

        public string Title { get; }

        public decimal TargetMoney { get; }

        public bool IsReached(decimal money) => money >= TargetMoney;

        public int GetProgress(decimal money)
        {
            if (money <= 0m)
            {
                return 0;
            }

            int percent = (int)(money * 100m / TargetMoney);

            return percent > 100 ? 100 : percent;
        }
    }
}
