using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        private const int WerehouseCapacity = 60;

        DetailsStore _detailsStore;

        Dictionary<DetailType, List<Detail>> _details;

        private int _quantityDetails;

        internal Warehouse()
        {
            _detailsStore = new DetailsStore();
            _details = FillDetails();
            _quantityDetails = _details.Count;
        }

        private Dictionary<DetailType, List<Detail>> FillDetails()
        {
            Dictionary<DetailType, List<Detail>> details = new Dictionary<DetailType, List<Detail>>();
            
            int initialQuantityDetails = 2;

            List<Detail> tempDetails = new List<Detail>();

            for (int i = 1; i <= _detailsStore.GetCountCatalogItems(); i++)
            {
                DetailType type = (DetailType)i;

                details.Add(type, new List<Detail>());

                for (int j = 0; j < initialQuantityDetails; j++)
                {
                    tempDetails.Add(_detailsStore.SellDetail((int)type));
                }

                details[type] = tempDetails;
            }

            return details;
        }

        private void AddDetail()
        {
            int userInput = Utils.ReadNumber();

            Detail detail = _detailsStore.SellDetail(userInput);

            if(detail != null)
            {
                _details[(DetailType)userInput].Add(detail);
            }
        }

        internal Detail SendPartToGarage(int userInput)
        {
            if (Utils.ContainsDetailType(userInput))
            {
                DetailType type = (DetailType)userInput;

                if (_details[type].Count != 0)
                {
                    Detail detail = _details[type][_details.Count - 1];

                    _details[type].RemoveAt(_details.Count - 1);

                    return detail;
                }
            }

            return null;
        }
    }
}