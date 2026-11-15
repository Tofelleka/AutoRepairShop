using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
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
                temp.Add(new Detail(data.type, data.name,data.price,data.replacementCost));
            }

            return temp;
        }

        private bool DetailIsBroken(Detail detail)
        {
            if(detail.IsBroken == true)
            {
                return true;
            }

            return false;
        }

        internal List<Detail> GetAllDetails()
        {
            List<Detail> temp = new List<Detail>();

            temp.AddRange(_details);

            return temp;
        }

        internal bool ReplacementPart(DetailType type) 
        {
            Detail replaceablePart = null;

            foreach(Detail detail in _details)
            {
                if(type == detail.Type)
                {
                    replaceablePart = detail;
                    break;
                }
            }

            return DetailIsBroken(replaceablePart);
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