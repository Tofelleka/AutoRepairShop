using System;

namespace AutoRepairShop
{
    internal class TuningOption
    {
        public TuningOption(
            CarBrand brand,
            string model,
            TuningType type,
            string title,
            string description,
            decimal price,
            DetailType partType,
            int requiredBoxLevel)
        {
            if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Модель и название тюнинга не могут быть пустыми.");
            }

            Brand = brand;
            Model = model;
            Type = type;
            Title = title;
            Description = description;
            Price = price;
            PartType = partType;
            RequiredBoxLevel = requiredBoxLevel;
        }

        public CarBrand Brand { get; }

        public string Model { get; }

        public TuningType Type { get; }

        public string Title { get; }

        public string Description { get; }

        public decimal Price { get; }

        public DetailType PartType { get; }

        public int RequiredBoxLevel { get; }
    }
}
