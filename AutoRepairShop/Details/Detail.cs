namespace AutoRepairShop
{
    internal class Detail
    {
        public Detail(DetailType type, string description, string name, decimal price, decimal replacementCost)
        {
            Type = type;
            Description = description;
            Name = name;
            Price = price;
            ReplacementCost = replacementCost;
            IsBroken = false;
        }

        public DetailType Type { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public decimal ReplacementCost { get; }
        public bool IsBroken { get; private set; }

        internal void BrokeDetail()
        {
            IsBroken = true;
        }
    }
}