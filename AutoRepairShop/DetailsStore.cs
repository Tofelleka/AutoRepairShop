using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class DetailsStore
    {
        private Dictionary<DetailType, Detail> _storeCatalog;

        internal DetailsStore()
        {
            _storeCatalog = FillCatalog();
        }

        private Dictionary<DetailType, Detail> FillCatalog()
        {
            var catalogData = DetailsCatalog.GetDetailsCatalog();

            if (catalogData == null)
            {
                throw new InvalidOperationException("Каталог деталей не был загружен.");
            }

            var details = new Dictionary<DetailType, Detail>();

            foreach (var data in catalogData)
            {
                details[data.type] = new Detail(data.type, data.description, data.name, data.price, data.replacementCost);
            }

            return details;
        }

        internal Detail SellDetail(DetailType type)
        {
            if (type != DetailType.Exit)
            {
                if (_storeCatalog.TryGetValue(type, out Detail detail))
                {
                    return new Detail(detail.Type, detail.Description, detail.Name, detail.Price, detail.ReplacementCost);
                }
            }

            return null;
        }

        internal Dictionary<DetailType, Detail> GetCatalog()
        {
            var catalog = new Dictionary<DetailType, Detail>(_storeCatalog.Count);

            foreach(var pair in _storeCatalog)
            {
                catalog[pair.Key] = pair.Value;
            }

            return catalog;
        }
    }
}