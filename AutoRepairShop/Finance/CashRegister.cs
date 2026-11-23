namespace AutoRepairShop
{
    internal class CashRegister
    {
        public CashRegister()
        {
            Money = 10000m;
        }

        public decimal Money { get; private set; }

        public void Apply(RepairReport report)
        {
            Money += report.Total;
        }

        public bool TryPay(decimal price)
        {
            if(price <= Money)
            {
                Money -= price;

                return true;
            }

            return false;
        }
    }
}