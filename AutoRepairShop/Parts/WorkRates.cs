namespace AutoRepairShop
{
    internal class WorkRates
    {
        private const decimal BaseRate = 1m;

        private readonly decimal _skillRate;
        private readonly decimal _bodyRate;
        private readonly decimal _electricRate;

        public WorkRates(decimal skillRate, decimal bodyRate, decimal electricRate)
        {
            _skillRate = skillRate;
            _bodyRate = bodyRate;
            _electricRate = electricRate;
        }

        public decimal GetRate(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Body:
                    return BaseRate + _skillRate + _bodyRate;

                case PartCategory.Electric:
                    return BaseRate + _skillRate + _electricRate;

                default:
                    return BaseRate + _skillRate;
            }
        }
    }
}
