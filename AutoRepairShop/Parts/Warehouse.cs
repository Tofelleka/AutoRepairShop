using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class Warehouse
    {
        private readonly Dictionary<DetailType, List<Detail>> _details;

        public Warehouse(Dictionary<DetailType, List<Detail>> details, int capacity)
        {
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            if (capacity <= 0)
            {
                throw new ArgumentException("Вместимость склада должна быть больше нуля.", nameof(capacity));
            }

            _details = details;
            Capacity = capacity;

            if (TotalQuantity > Capacity)
            {
                throw new ArgumentException(
                    $"Начальный запас больше вместимости склада: {TotalQuantity} из {Capacity}.",
                    nameof(details));
            }
        }

        public int Capacity { get; private set; }

        public int TotalQuantity => _details.Values.Sum(detail => detail.Count);

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

            if (TotalQuantity >= Capacity)
            {
                return false;
            }

            details.Add(detail);

            return true;
        }

        public bool TryTake(DetailType type, out Detail detail)
        {
            detail = null;

            if (_details.TryGetValue(type, out List<Detail> details) == false)
            {
                return false;
            }

            if (details.Count == 0)
            {
                return false;
            }

            detail = details[details.Count - 1];

            details.RemoveAt(details.Count - 1);

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

        public void IncreaseCapacity(int extra)
        {
            if (extra <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(extra), "Прибавка вместимости должна быть положительной.");
            }

            Capacity += extra;
        }
    }
}