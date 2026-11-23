namespace AutoRepairShop
{
    internal class RepairReport
    {
        public RepairReport(decimal revenue, decimal fine)
        {
            Revenue = revenue;
            Fine = fine;
        }

        public decimal Revenue { get; }
        public decimal Fine { get; }
        public decimal Total => Revenue - Fine;
    }
}
