using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
        private double _chanceBreakdown = 0.3;

        private List<string> _names = new List<string>()
        {
            {"ВАЗ - 2108" },
            {"ВАЗ - 2109" },
            {"ВАЗ - 21091" },
            {"ВАЗ - 21093" },
            {"ВАЗ - 21093i" },
            {"ВАЗ - 2113" },
            {"ВАЗ - 2114" },
            {"ВАЗ - 2115" }
        };
        private List<Detail> _details;

        public Car()
        {
            _details = FillDetails();
            Name = _names[Utils.GetRandomNumber(0,_names.Count)];
        }

        public string Name { get; }

        private List<Detail> FillDetails()
        {
            List<Detail> temp = new List<Detail>();

            foreach(var data in DetailsCatalog.GetDetailsCatalog())
            {
               Detail detail = new Detail(data.type, data.description, data.name,data.price,data.replacementCost);

                if (Utils.GetRandomBool(_chanceBreakdown))
                {
                    detail.BrokeDetail();
                }

                temp.Add(detail);
            }

            return temp;
        }

        internal List<Detail> GetAllDetails()
        {
            List<Detail> temp = new List<Detail>();

            temp.AddRange(_details);

            return temp;
        }

        internal bool ReplacementPart(Detail newDetail) 
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if(newDetail.Type == _details[i].Type)
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
            foreach(Detail detail in _details)
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