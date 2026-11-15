using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        private const int WerehouseCapacity = 60;

        DetailsStore _detailsStore;

        List<Detail> _details;

        private int _quantityDetails;

        internal Warehouse()
        {
            _detailsStore = new DetailsStore();
            _details = FillDetails();
            _quantityDetails = _details.Count;
        }

        private List<Detail> FillDetails()
        {
            List<Detail> details = new List<Detail>();

            int initialQuantityDetails = 2;

            for (int i = 1; i <= _detailsStore.GetCountCatalogItems(); i++)
            {
                for (int j = 0; j < initialQuantityDetails; j++)
                {
                    details.Add(_detailsStore.SellDetail(i));
                }
            }

            return details;
        }
    }
}
