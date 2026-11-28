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

        public Car Create(CarBrand brand, Season season)
        {
            List<string> names = new List<string>(CarNames.GetNames(brand));

            if (names.Count == 0)
            {
                throw new InvalidOperationException("Список названий машин пуст.");
            }

            string name = names[_random.Next(0, names.Count)];

            return CreateForModel(brand, name, season);
        }

        public Car CreateForModel(CarBrand brand, string model, Season season)
        {
            var details = new Dictionary<DetailType, Detail>();

            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(brand, model))
            {
                if (template.Category == PartCategory.Material || template.Category == PartCategory.Tuning)
                {
                    continue;
                }

                Brand partBrand = DetailsCatalog.Brands[_random.Next(0, DetailsCatalog.Brands.Count)];
                var detail = new Detail(template, partBrand);

                double baseChance = template.FaultChance ?? GetBaseChance(template.Category, template.Type, season);
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

            var car = new Car(brand, model, details);

            double tireChance = season == Season.Spring || season == Season.Autumn ? 0.55d : 0.05d;

            if (_random.NextChance(tireChance))
            {
                car.NeedTireChange();
            }

            return car;
        }

        private static double GetBaseChance(PartCategory category, DetailType type, Season season)
        {
            if (type == DetailType.Heater)
            {
                switch (season)
                {
                    case Season.Winter:
                        return 0.45d;

                    case Season.Autumn:
                        return 0.30d;

                    case Season.Spring:
                        return 0.10d;

                    default:
                        return 0.05d;
                }
            }

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
