using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class WarehouseFactory
    {
        private const int InitialQuantityPerType = 2;

        private readonly PartsFactory _partsFactory;

        public WarehouseFactory(PartsFactory partsFactory)
        {
            if (partsFactory == null)
            {
                throw new ArgumentNullException(nameof(partsFactory));
            }

            _partsFactory = partsFactory;
        }

        public Warehouse Create()
        {
            var quantities = new Dictionary<DetailType, int>();

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                quantities[type] = InitialQuantityPerType;
            }

            return Create(Warehouse.DefaultCapacity, quantities);
        }

        public Warehouse Create(int capacity, IReadOnlyDictionary<DetailType, int> quantities)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Вместимость склада должна быть больше нуля.", nameof(capacity));
            }

            if (quantities == null)
            {
                throw new ArgumentNullException(nameof(quantities));
            }

            var details = new Dictionary<DetailType, List<Detail>>();
            int totalQuantity = 0;

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                var detailList = new List<Detail>();
                int quantity = quantities.TryGetValue(type, out int savedQuantity) ? savedQuantity : 0;

                for (int i = 0; i < quantity && totalQuantity < capacity; i++)
                {
                    detailList.Add(_partsFactory.Create(type));
                    totalQuantity++;
                }

                details[type] = detailList;
            }

            return new Warehouse(details, capacity);
        }
    }
}
