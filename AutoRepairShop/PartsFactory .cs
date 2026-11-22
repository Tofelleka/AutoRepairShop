using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class PartsFactory
    {
        private Dictionary<DetailType, Detail> _storeCatalog;

        internal PartsFactory()
        {
            _storeCatalog = new Dictionary<DetailType, Detail>(DetailsCatalog.GetDetailsCatalog());
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
            return new Dictionary<DetailType, Detail>(_storeCatalog);
        }
    }
}