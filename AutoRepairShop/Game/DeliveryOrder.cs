using System;

namespace AutoRepairShop
{
    internal class DeliveryOrder
    {
        public DeliveryOrder(DetailType type, string brandName, int arrivalDay)
        {
            if (string.IsNullOrEmpty(brandName))
            {
                throw new ArgumentException("Название бренда не может быть пустым.", nameof(brandName));
            }

            Type = type;
            BrandName = brandName;
            ArrivalDay = arrivalDay;
        }

        public DetailType Type { get; }

        public string BrandName { get; }

        public int ArrivalDay { get; }
    }
}
