using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        internal const int WarehouseCapacity = 60;

        private readonly PartsFactory _partsFactory;

        private readonly Dictionary<DetailType, List<Detail>> _details;

        internal Warehouse(PartsFactory partsFactory)
        {
            if (partsFactory == null)
            {
                throw new ArgumentNullException(nameof(partsFactory));
            }

            _partsFactory = partsFactory;
            _details = FillDetails();
        }

        public int TotalQuantity 
        {
            get
            {
                int count = 0;

                foreach(var pair in _details)
                {
                    count += pair.Value.Count;
                }

                return count;
            }
        }

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
                    Detail newDetail = _partsFactory.SellDetail(type);

                    if (TotalQuantity < WarehouseCapacity && newDetail != null)
                    {
                        tempDetails.Add(newDetail);
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

                return detail;
            }

            return null;
        }

        internal bool TryAdd()
        {
            if(TotalQuantity < WarehouseCapacity)
            {
                return true;
            }

            return false;
        }

        internal void AddDetail(Detail detail)
        {
            _details[detail.Type].Add(detail);
        }
    }
}