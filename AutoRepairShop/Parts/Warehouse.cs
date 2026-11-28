using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        public const int WarehouseCapacity = 60;

        private readonly Dictionary<DetailType, List<Detail>> _details;

        public Warehouse(Dictionary<DetailType, List<Detail>> details)
        {
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            _details = details;

            if (TotalQuantity > WarehouseCapacity)
            {
                throw new ArgumentException(
                    $"Начальный запас больше вместимости склада: {TotalQuantity} из {WarehouseCapacity}.",
                    nameof(details));
            }
        }

        public int TotalQuantity => _details.Values.Sum(details => details.Count);

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
    }
}