using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class CarFactory
    {
        private const double DefaultBreakdownChance = 0.3;

        private readonly IRandomSource _random;

        private readonly IReadOnlyDictionary<DetailType, Detail> _partsCatalog;

        public CarFactory(Dictionary<DetailType, Detail> partsCatalog, IRandomSource random)
        {
            if (partsCatalog == null)
            {
                throw new ArgumentNullException(nameof(partsCatalog));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _random = random;
            _partsCatalog = partsCatalog;
        }

        public Car Create() => new Car(CarNames.AllNames[_random.Next(0, CarNames.AllNames.Count)], FillDetails());

        private Dictionary<DetailType, Detail> FillDetails()
        {
            var details = new Dictionary<DetailType, Detail>();

            if (_partsCatalog.Count == 0)
            {
                throw new InvalidOperationException("Каталог деталей пуст");
            }

            foreach (var pair in _partsCatalog)
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

            if (HasBrokenDetail(details) == false)
            {
                var types = new List<DetailType>(details.Keys);

                details[types[_random.Next(0, types.Count)]].BrokeDetail();
            }

            return details;
        }

        private bool HasBrokenDetail(Dictionary<DetailType, Detail> details)
        {
            foreach (var pair in details)
            {
                if (pair.Value.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
