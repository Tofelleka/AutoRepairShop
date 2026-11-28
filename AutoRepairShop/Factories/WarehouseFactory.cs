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

            foreach (PartTemplate template in DetailsCatalog.Templates)
            {
                quantities[template.Type] = InitialQuantityPerType;
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

            foreach (PartTemplate template in DetailsCatalog.Templates)
            {
                details[template.Type] = new List<Detail>();
            }

            int totalQuantity = 0;

            foreach (KeyValuePair<DetailType, int> pair in quantities)
            {
                if (DetailsCatalog.TryGetTemplate(pair.Key, out PartTemplate _) == false)
                {
                    continue;
                }

                List<Detail> detailList = details[pair.Key];

                for (int i = 0; i < pair.Value && totalQuantity < capacity; i++)
                {
                    detailList.Add(_partsFactory.Create(pair.Key));
                    totalQuantity++;
                }
            }

            return new Warehouse(details, capacity);
        }
    }
}
