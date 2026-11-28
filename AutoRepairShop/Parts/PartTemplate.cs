using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class PartTemplate
    {
        public PartTemplate(
            DetailType type,
            PartCategory category,
            string name,
            string description,
            decimal basePrice,
            decimal baseWork,
            DetailType? relatedType = null,
            CarBrand? brand = null,
            double? faultChance = null,
            decimal extraWork = 0m,
            IReadOnlyList<string> models = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название детали не может быть пустым.", nameof(name));
            }

            if (basePrice <= 0m || baseWork <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(basePrice), "Цены должны быть положительными.");
            }

            Type = type;
            Category = category;
            Name = name;
            Description = description;
            BasePrice = basePrice;
            BaseWork = baseWork;
            RelatedType = relatedType;
            Brand = brand;
            FaultChance = faultChance;
            ExtraWork = extraWork;
            Models = models ?? new List<string>();
        }

        public DetailType Type { get; }

        public PartCategory Category { get; }

        public string Name { get; }

        public string Description { get; }

        public decimal BasePrice { get; }

        public decimal BaseWork { get; }

        public DetailType? RelatedType { get; }

        public CarBrand? Brand { get; }

        public double? FaultChance { get; }

        public decimal ExtraWork { get; }

        public IReadOnlyList<string> Models { get; }

        public bool FitsModel(string model)
        {
            if (Models.Count == 0)
            {
                return true;
            }

            foreach (string item in Models)
            {
                if (item == model)
                {
                    return true;
                }
            }

            return false;
        }

        public decimal GetPrice(Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            return Math.Round(BasePrice * brand.PriceFactor, 0, MidpointRounding.AwayFromZero);
        }

        public decimal GetWork(Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            return Math.Round(BaseWork * brand.WorkFactor, 0, MidpointRounding.AwayFromZero);
        }
    }
}
