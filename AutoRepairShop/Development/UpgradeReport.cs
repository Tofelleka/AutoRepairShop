namespace AutoRepairShop
{
    internal class UpgradeReport
    {
        public UpgradeReport(UpgradeOutcome outcome, UpgradeType type, string name, decimal price, int level)
        {
            Outcome = outcome;
            Type = type;
            Name = name;
            Price = price;
            Level = level;
        }

        public UpgradeOutcome Outcome { get; }

        public UpgradeType Type { get; }

        public string Name { get; }

        public decimal Price { get; }

        public int Level { get; }
    }
}
