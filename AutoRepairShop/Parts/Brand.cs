using System;

namespace AutoRepairShop
{
    internal class Brand
    {
        public Brand(string name, decimal priceFactor, decimal workFactor, double faultFactor)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название производителя не может быть пустым.", nameof(name));
            }

            if (priceFactor <= 0m || workFactor <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(priceFactor), "Коэффициенты должны быть положительными.");
            }

            if (faultFactor <= 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(faultFactor), "Коэффициент поломок должен быть положительным.");
            }

            Name = name;
            PriceFactor = priceFactor;
            WorkFactor = workFactor;
            FaultFactor = faultFactor;
        }

        public string Name { get; }

        public decimal PriceFactor { get; }

        public decimal WorkFactor { get; }

        public double FaultFactor { get; }
    }
}
