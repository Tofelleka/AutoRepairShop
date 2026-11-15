namespace AutoRepairShop
{
    internal class Detail
    {
        private double _chanceBreakdown = 0.3;

        public Detail(DetailType type, string name, int price, int replacementCost)
        {
            Type = type;
            Name = name;
            Price = price;
            ReplacementCost = replacementCost;
            IsBroken = Utils.GetRandomBool(_chanceBreakdown);
        }

        public Detail(DetailType type, string description, string name, int price, int replacementCost) : this(type, name, price, replacementCost)
        {
            Type = type;
            Description = description;
            Name = name;
            Price = price;
            ReplacementCost = replacementCost;
            IsBroken = Utils.GetRandomBool(_chanceBreakdown);
        }

        public DetailType Type { get; }
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
        public int ReplacementCost { get; }
        public bool IsBroken { get; }
    }
}