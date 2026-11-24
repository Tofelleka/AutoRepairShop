using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        public const int WarehouseCapacity = 60;

        private readonly PartsFactory _partsFactory;
        private readonly Dictionary<DetailType, List<Detail>> _details;

        public Warehouse(PartsFactory partsFactory)
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

                foreach (var pair in _details)
                {
                    count += pair.Value.Count;
                }

                return count;
            }
        }

        public bool TryTake(DetailType type, out Detail newDetail)
        {
            newDetail = null;

            if (_details.TryGetValue(type, out List<Detail> details) == false)
            {
                return false;
            }

            if (details.Count == 0)
            {
                return false;
            }

            int lastIndex = details.Count - 1;

            newDetail = details[lastIndex];

            details.RemoveAt(lastIndex);

            return true;
        }

        public int GetQuantityDetail(DetailType type)
        {
            if (_details.TryGetValue(type, out List<Detail> details) == false)
            {
                throw new ArgumentException($"Неизвестный тип детали: {type}.", nameof(type));
            }

            return details.Count;
        }

        public bool TryAdd(Detail detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException(nameof(detail));
            }

            if (_details.TryGetValue(detail.Type, out List<Detail> details) == false)
            {
                return false;
            }

            if (TotalQuantity >= WarehouseCapacity)
            {
                return false;
            }

            details.Add(detail);

            return true;
        }

        private Dictionary<DetailType, List<Detail>> FillDetails()
        {
            var details = new Dictionary<DetailType, List<Detail>>(Enum.GetValues(typeof(DetailType)).Length);

            const int initialQuantityDetails = 2;

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                var tempDetails = new List<Detail>(initialQuantityDetails);

                for (int i = 0; i < initialQuantityDetails; i++)
                {
                    Detail newDetail = _partsFactory.CreateNewDetail(type);

                    tempDetails.Add(newDetail);
                }

                details[type] = tempDetails;
            }

            return details;
        }
    }
}