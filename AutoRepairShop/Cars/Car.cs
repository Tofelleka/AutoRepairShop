using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
        private const double DefaultBreakdownChance = 0.3;

        private static readonly List<string> Names = new List<string>
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

        private readonly PartsFactory _partsFactory;

        private readonly List<Detail> _details;

        public Car(PartsFactory partsFactory)
        {
            if (partsFactory == null)
            {
                throw new ArgumentNullException(nameof(partsFactory));
            }

            _partsFactory = partsFactory;
            _details = FillDetails();
            Name = Names[Utils.GetRandomNumber(Utils.FirstIndex, Names.Count)];
        }

        public string Name { get; }

        internal List<Detail> GetAllDetails()
        {
            return new List<Detail>(_details);
        }

        internal void ReplacementPart(Detail newDetail)
        {
            for (int i = Utils.FirstIndex; i < _details.Count; i++)
            {
                if (newDetail.Type == _details[i].Type)
                {
                    _details[i] = newDetail;

                    return;
                }
            }
        }

        internal bool CarIsBroken()
        {
            foreach (Detail detail in _details)
            {
                if (detail.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Detail> FillDetails()
        {
            var details = new List<Detail>();
            var catalog = _partsFactory.GetCatalog();

            if (catalog == null || catalog.Count == Utils.FirstIndex)
            {
                throw new InvalidOperationException("Каталог деталей пуст");
            }

            foreach (var data in catalog)
            {
                var detail = new Detail(
                    data.Value.Type,
                    data.Value.Description,
                    data.Value.Name,
                    data.Value.Price,
                    data.Value.ReplacementCost);

                if (Utils.GetRandomBool(DefaultBreakdownChance))
                {
                    detail.BrokeDetail();
                }

                details.Add(detail);
            }

            if (HasBrokenDetail(details) == false)
            {
                details[Utils.GetRandomNumber(Utils.FirstIndex, details.Count)].BrokeDetail();
            }

            return details;
        }

        private static bool HasBrokenDetail(List<Detail> details)
        {
            foreach (Detail detail in details)
            {
                if (detail.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
