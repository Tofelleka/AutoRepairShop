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
            var details = new Dictionary<DetailType, List<Detail>>();
            int totalQuantity = 0;

            foreach (DetailType type in Enum.GetValues(typeof(DetailType)))
            {
                var detailList = new List<Detail>();

                for (int i = 0; i < InitialQuantityPerType && totalQuantity < Warehouse.WarehouseCapacity; i++)
                {
                    detailList.Add(_partsFactory.Create(type));
                    totalQuantity++;
                }

                details[type] = detailList;
            }

            return new Warehouse(details);
        }
    }
}