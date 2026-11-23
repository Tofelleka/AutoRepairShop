using System;
using System.Collections.Generic;
using AutoRepairShop.Common;

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

        private readonly IRandomSource _random;
        private readonly Dictionary<DetailType, Detail> _details;

        public Car(Dictionary<DetailType, Detail> partsCatalog, IRandomSource random)
        {
            if (partsCatalog == null)
            {
                throw new ArgumentNullException(nameof(partsCatalog));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            var _partsCatalog = partsCatalog;
            _random = random;
            _details = FillDetails();
            Name = Names[_random.Next(0, Names.Count)];
        }

        public string Name { get; }

        internal List<Detail> GetAllDetails()
        {
            var details = new List<Detail>();

            foreach (var pair in _details)
            {
                details.Add(pair.Value);
            }

            return details;
        }

        internal void ReplacementPart(Detail newDetail)
        {
            _details[newDetail.Type] = newDetail;
        }

        internal bool CarIsBroken()
        {
            foreach (var pair in _details)
            {
                if (pair.Value.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }

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