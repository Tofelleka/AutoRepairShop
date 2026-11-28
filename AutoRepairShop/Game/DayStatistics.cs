namespace AutoRepairShop
{
    internal class DayStatistics
    {
        public DayStatistics(
            int cars,
            int refused,
            int repaired,
            decimal revenue,
            decimal fines,
            decimal rent,
            decimal purchaseCosts)
        {
            Cars = cars;
            Refused = refused;
            Repaired = repaired;
            Revenue = revenue;
            Fines = fines;
            Rent = rent;
            PurchaseCosts = purchaseCosts;
        }

        public int Cars { get; }

        public int Refused { get; }

        public int Repaired { get; }

        public decimal Revenue { get; }

        public decimal Fines { get; }

        public decimal Rent { get; }

        public decimal PurchaseCosts { get; }

        public decimal NetRevenue => Revenue - Fines;

        public decimal Profit => NetRevenue - Rent - PurchaseCosts;
    }
}
