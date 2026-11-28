using System;

namespace AutoRepairShop
{
    internal class CashRegister
    {
        public const decimal StartMoney = 100_000m;

        public CashRegister() : this(StartMoney)
        {
        }

        public CashRegister(decimal startMoney)
        {
            if (startMoney < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(startMoney), "Начальная сумма не может быть отрицательной.");
            }

            Money = startMoney;
        }

        public decimal Money { get; private set; }

        public bool CanAfford(decimal amount) => Money >= amount;

        public void Apply(decimal amount) => Money += amount;

        public void Pay(decimal amount)
        {
            if (CanAfford(amount) == false)
            {
                throw new InvalidOperationException("В кассе недостаточно денег.");
            }

            Money -= amount;
        }
    }
}
