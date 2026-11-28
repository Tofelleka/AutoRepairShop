using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class CarFactory
    {
        private const double DefaultBreakdownChance = 0.3;

        private readonly IRandomSource _random;
        private readonly IReadOnlyDictionary<DetailType, Detail> _catalog;

        public CarFactory(IReadOnlyDictionary<DetailType, Detail> catalog, IRandomSource random)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(nameof(catalog));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _catalog = catalog;
            _random = random;
        }

        public Car Create()
        {
            if (_catalog.Count == 0)
            {
                throw new InvalidOperationException("Каталог деталей пуст.");
            }

            if (CarNames.AllNames.Count == 0)
            {
                throw new InvalidOperationException("Список названий машин пуст.");
            }

            string name = CarNames.AllNames[_random.Next(0, CarNames.AllNames.Count)];

            return new Car(name, FillDetails());
        }

        private Dictionary<DetailType, Detail> FillDetails()
        {
            var details = new Dictionary<DetailType, Detail>();

            foreach (var pair in _catalog)
            {
                var detail = new Detail(pair.Value.Type,
                    pair.Value.Description,
                    pair.Value.Name,
                    pair.Value.Price,
                    pair.Value.ReplacementCost);

                if (_random.NextChance(DefaultBreakdownChance))
                {
                    detail.BrokeDetail();
                }

                details[detail.Type] = detail;
            }

            if (details.Values.Any(detail => detail.IsBroken) == false)
            {
                Detail randomDetail = details.Values.ElementAt(_random.Next(0, details.Count));

                randomDetail.BrokeDetail();
            }

            return details;
        }
    }
}
