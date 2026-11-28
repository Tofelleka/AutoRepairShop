using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class CarFactory
    {
        private const double RepairOptionChance = 0.4d;

        private readonly IRandomSource _random;

        public CarFactory(IRandomSource random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _random = random;
        }

        public Car Create(CarBrand brand)
        {
            List<string> names = new List<string>(CarNames.GetNames(brand));

            if (names.Count == 0)
            {
                throw new InvalidOperationException("Список названий машин пуст.");
            }

            var details = new Dictionary<DetailType, Detail>();
            string name = names[_random.Next(0, names.Count)];

            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(brand, name))
            {
                if (template.Category == PartCategory.Material)
                {
                    continue;
                }

                Brand partBrand = DetailsCatalog.Brands[_random.Next(0, DetailsCatalog.Brands.Count)];
                var detail = new Detail(template, partBrand);

                double baseChance = template.FaultChance ?? GetBaseChance(template.Category);
                double chance = baseChance * partBrand.FaultFactor;

                if (_random.NextChance(chance))
                {
                    detail.BrokeDetail();

                    if (DetailsCatalog.CanRepair(template.Type) && _random.NextChance(RepairOptionChance))
                    {
                        detail.MarkRepairable();
                    }
                }

                details[detail.Type] = detail;
            }

            if (details.Values.Any(detail => detail.IsBroken) == false)
            {
                var types = new List<DetailType>(details.Keys);

                details[types[_random.Next(0, types.Count)]].BrokeDetail();
            }

            return new Car(brand, name, details);
        }

        private static double GetBaseChance(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Engine:
                    return 0.20d;

                case PartCategory.Body:
                    return 0.12d;

                case PartCategory.Electric:
                    return 0.10d;

                default:
                    return 0.12d;
            }
        }
    }
}
