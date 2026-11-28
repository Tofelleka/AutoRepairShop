namespace AutoRepairShop
{
    internal class ShiftStatistics
    {
        public ShiftStatistics(
            int days,
            int cars,
            int repaired,
            int refused,
            decimal revenue,
            decimal fines,
            decimal rent,
            decimal purchaseCosts,
            decimal upgradeCosts,
            int referrals)
        {
            Days = days;
            Cars = cars;
            Repaired = repaired;
            Refused = refused;
            Revenue = revenue;
            Fines = fines;
            Rent = rent;
            PurchaseCosts = purchaseCosts;
            UpgradeCosts = upgradeCosts;
            Referrals = referrals;
        }

        public int Days { get; }

        public int Cars { get; }

        public int Repaired { get; }

        public int Refused { get; }

        public decimal Revenue { get; }

        public decimal Fines { get; }

        public decimal Rent { get; }

        public decimal PurchaseCosts { get; }

        public decimal UpgradeCosts { get; }

        public int Referrals { get; }

        public decimal NetRevenue => Revenue - Fines;

        public decimal Profit => NetRevenue - Rent - PurchaseCosts - UpgradeCosts;
    }
}
