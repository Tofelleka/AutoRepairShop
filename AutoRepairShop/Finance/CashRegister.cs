using System;

namespace AutoRepairShop
{
    internal class CashRegister
    {
        private const decimal StartMoney = 10000m;

        public CashRegister() => Money = StartMoney;

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