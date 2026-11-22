namespace AutoRepairShop
{
    internal class CashRegister
    {
        internal CashRegister()
        {
            Money = 10000m;
        }

        internal decimal Money { get; private set; }

        internal void Apply(RepairReport report)
        {
            Money += report.Total;
        }
    }
}