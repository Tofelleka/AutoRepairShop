using System;

namespace AutoRepairShop
{
    internal class CourseInfo
    {
        public CourseInfo(CourseType type, string name, CarBrand brand, decimal requiredEarned, decimal price)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название курса не может быть пустым.", nameof(name));
            }

            if (requiredEarned < 0m || price < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Суммы не могут быть отрицательными.");
            }

            Type = type;
            Name = name;
            Brand = brand;
            RequiredEarned = requiredEarned;
            Price = price;
        }

        public CourseType Type { get; }

        public string Name { get; }

        public CarBrand Brand { get; }

        public decimal RequiredEarned { get; }

        public decimal Price { get; }
    }
}
