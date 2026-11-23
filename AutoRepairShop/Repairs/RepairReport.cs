namespace AutoRepairShop
{
    internal class RepairReport
    {
        internal RepairReport(decimal revenue, decimal fine)
        {
            Revenue = revenue;
            Fine = fine;
        }

        internal decimal Revenue { get; }
        internal decimal Fine { get; }
        internal decimal Total
        {
            get
            {
                return Revenue - Fine;
            }
        }
    }
}
