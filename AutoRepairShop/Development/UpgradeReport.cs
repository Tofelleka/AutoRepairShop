namespace AutoRepairShop
{
    internal class UpgradeReport
    {
        public UpgradeReport(UpgradeOutcome outcome, decimal price, int level)
        {
            Outcome = outcome;
            Price = price;
            Level = level;
        }

        public UpgradeOutcome Outcome { get; }

        public decimal Price { get; }

        public int Level { get; }
    }
}