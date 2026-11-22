using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        internal const int WarehouseCapacity = 60;

        private readonly DetailsStore _detailsStore;

        private readonly Dictionary<DetailType, List<Detail>> _details;

        internal Warehouse(DetailsStore detailsStore)
        {
            if (detailsStore == null)
            {
                throw new ArgumentNullException(nameof(detailsStore));
            }

            _detailsStore = detailsStore;
            _details = FillDetails();
        }

        public int TotalQuantity { get; private set; }

        private Dictionary<DetailType, List<Detail>> FillDetails()
        {
            var details = new Dictionary<DetailType, List<Detail>>(Enum.GetValues(typeof(DetailType)).Length);

            const int initialQuantityDetails = 2;

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                if (type == DetailType.Exit)
                {
                    continue;
                }

                var tempDetails = new List<Detail>(initialQuantityDetails);

                for (int i = 0; i < initialQuantityDetails; i++)
                {
                    Detail newDetail = _detailsStore.SellDetail(type);

                    if (TotalQuantity < WarehouseCapacity && newDetail != null)
                    {
                        tempDetails.Add(newDetail);

                        TotalQuantity++;
                    }
                    else break;
                }

                details[type] = tempDetails;
            }

            return details;
        }

        internal Detail SendPartToGarage(DetailType type)
        {
            if (_details.TryGetValue(type, out List<Detail> detailOfType) && detailOfType.Count > 0)
            {
                int lastIndex = detailOfType.Count - 1;

                Detail detail = detailOfType[lastIndex];

                detailOfType.RemoveAt(lastIndex);

                TotalQuantity--;

                return detail;
            }

            return null;
        }
    }
}