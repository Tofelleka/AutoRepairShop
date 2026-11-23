using System.Collections.Generic;
using System.ComponentModel;

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
            if (type == DetailType.Exit)
            {
                throw new InvalidEnumArgumentException(nameof(type));
            }

                Detail detail = _storeCatalog[type];

                return new Detail(detail.Type, detail.Description, detail.Name, detail.Price, detail.ReplacementCost);
        }

        internal Dictionary<DetailType, Detail> GetCatalog()
        {
            return new Dictionary<DetailType, Detail>(_storeCatalog);
        }
    }
}