using System;
using System.Collections.Generic;
using AutoRepairShop.Common;

namespace AutoRepairShop.Factorys
{
    internal class CarNames
    {
        public IReadOnlyList<string> AllNames = new List<string>
        {
            "ВАЗ - 2108",
            "ВАЗ - 2109",
            "ВАЗ - 21091",
            "ВАЗ - 21093",
            "ВАЗ - 21093i",
            "ВАЗ - 2113",
            "ВАЗ - 2114",
            "ВАЗ - 2115"
        };
    }

    internal class CarFactory
    {
        private const double DefaultBreakdownChance = 0.3;

        private readonly IRandomSource _random;

        private readonly CarNames _carNames;

        Dictionary<DetailType, Detail> _partsCatalog;

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

            _carNames = new CarNames();
            _random = random;
            _partsCatalog = partsCatalog;
        }

        public Car GetNewCar()
        {
            Car car = new Car(GetRandomName(), FillDetails());

            if(car == null)
            {
                throw new ArgumentNullException("Машина пуста");
            }

            return car;
        }

        private string GetRandomName() => _carNames.AllNames[_random.Next(0, _carNames.AllNames.Count)];

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

        private static bool HasBrokenDetail(Dictionary<DetailType, Detail> details)
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
