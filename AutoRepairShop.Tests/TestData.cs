using System.Collections.Generic;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal class AlwaysHealthyRandom : IRandomSource
    {
        public int Next(int minValue, int maxValue) => minValue;

        public int NextInclusive(int minValue, int maxValue) => minValue;

        public bool NextChance(double chance) => false;
    }

    internal class AlwaysRiskyRandom : IRandomSource
    {
        public int Next(int minValue, int maxValue) => minValue;

        public int NextInclusive(int minValue, int maxValue) => minValue;

        public bool NextChance(double chance) => true;
    }

    internal static class TestData
    {
        public static int CarPartCount(CarBrand brand)
        {
            int count = 0;

            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(brand))
            {
                if (template.Category != PartCategory.Material && template.Category != PartCategory.Tuning)
                {
                    count++;
                }
            }

            return count;
        }

        public static Dictionary<DetailType, Detail> CreateDetails()
        {
            var details = new Dictionary<DetailType, Detail>();

            foreach (PartTemplate template in DetailsCatalog.Templates)
            {
                details[template.Type] = new Detail(template, DetailsCatalog.StandardBrand);
            }

            return details;
        }

        public static Car CreateCarWithBroken(params DetailType[] brokenTypes)
        {
            Dictionary<DetailType, Detail> details = CreateDetails();

            foreach (DetailType type in brokenTypes)
            {
                details[type].BrokeDetail();
            }

            return new Car(CarBrand.Vaz, "Тестовая", details);
        }

        public static Warehouse CreateWarehouse() => new WarehouseFactory(new PartsFactory()).Create();

        public static Warehouse CreateEmptyWarehouse() =>
            new WarehouseFactory(new PartsFactory()).Create(10, new Dictionary<DetailType, int>());

        public static Garage CreateGarage(Warehouse warehouse) =>
            new Garage(warehouse, new AlwaysHealthyRandom());

        public static decimal StandardPayout(DetailType type, decimal workRate) =>
            new Detail(DetailsCatalog.GetTemplate(type), DetailsCatalog.StandardBrand).GetPayout(workRate);
    }
}
