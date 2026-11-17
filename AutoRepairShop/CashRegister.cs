namespace AutoRepairShop
{
    internal class CashRegister
    {
        Garage _garage;

        internal CashRegister(Garage garage)
        {
            _garage = garage;
            Money = 10000;
        }

        public int Money { get; private set; }

        internal void CalculateProfitFromRepair(int income)
        {
            Money += income;
        }
    }
}