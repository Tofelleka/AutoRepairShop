using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
        private const double DefaultBreakdownChance = 0.3;

        private DetailsStore _detailsStore;

        private List<string> _names = new List<string>
        {
            "ВАЗ - 2108" ,
            "ВАЗ - 2109" ,
            "ВАЗ - 21091" ,
            "ВАЗ - 21093" ,
            "ВАЗ - 21093i" ,
            "ВАЗ - 2113" ,
            "ВАЗ - 2114" ,
            "ВАЗ - 2115"
        };

        private List<Detail> _details;

        public Car(DetailsStore detailsStore)
        {
            if(detailsStore == null)
            {
                throw new ArgumentNullException(nameof(detailsStore),"Пустой аргумент");
            }

            _detailsStore = detailsStore;
            _details = FillDetails();
            Name = _names[Utils.GetRandomNumber(0, _names.Count)];
        }

        public string Name { get; }

        private List<Detail> FillDetails()
        {
            var temp = new List<Detail>();
            var catalog = _detailsStore.GetCatalog();

            if (catalog == null || catalog.Count == 0)
            {
                return temp;
            } 

            foreach (var data in catalog)
            {
                var detail = new Detail(data.Value.Type, data.Value.Description, data.Value.Name, data.Value.Price,data.Value.ReplacementCost);

                if (Utils.GetRandomBool(DefaultBreakdownChance))
                {
                    detail.BrokeDetail();
                }

                temp.Add(detail);
            }

            return temp;
        }

        internal List<Detail> GetAllDetails()
        {
            return new List<Detail>(_details);
        }

        internal bool ReplacementPart(Detail newDetail)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if (newDetail.Type == _details[i].Type)
                {
                    bool isBroken = _details[i].IsBroken;

                    _details[i] = newDetail;

                    return isBroken;
                }
            }

            return false;
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

        internal bool IsDetailBroken(DetailType type)
        {
            foreach (Detail detail in _details)
            {
                if (detail.Type == type)
                {
                    return detail.IsBroken;
                }
            }

            return false;
        }
    }
}