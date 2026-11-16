using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class DetailsStore
    {
        private List<Detail> _storeCatalog;

        internal DetailsStore()
        {
            _storeCatalog = FillCatalog();
        }

        private List<Detail> FillCatalog()
        {
            List<(DetailType type, string name, int price, int replacementCost, string description)> tempData = DetailsCatalog.GetDetailsCatalog();

            List<Detail> details = new List<Detail>();

            foreach (var data in tempData)
            {
                details.Add(new Detail(data.type, data.description, data.name, data.price, data.replacementCost));
            }

            return details;
        }

        internal int GetCountCatalogItems()
        {
            return _storeCatalog.Count;
        }

        internal Detail SellDetail(int userInput)
        {
            if (Utils.ContainsDetailType(userInput))
            {
                foreach (Detail detail in _storeCatalog)
                {
                    if ((DetailType)userInput == detail.Type)
                    {
                        return new Detail(detail.Type, detail.Description, detail.Name, detail.Price, detail.ReplacementCost);
                    }
                }
            }

            return null;
        }
    }
}