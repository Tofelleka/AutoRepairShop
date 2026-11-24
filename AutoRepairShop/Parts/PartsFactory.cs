using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class PartsFactory
    {
        private readonly Dictionary<DetailType, Detail> _storeCatalog;

        public PartsFactory() => _storeCatalog = new Dictionary<DetailType, Detail>(DetailsCatalog.GetDetailsCatalog());

        public Detail CreateNewDetail(DetailType type)
        {
            Detail detail = _storeCatalog[type];

            return new Detail(detail.Type, detail.Description, detail.Name, detail.Price, detail.ReplacementCost);
        }

        public Dictionary<DetailType, Detail> GetCatalog() => new Dictionary<DetailType, Detail>(_storeCatalog);
    }
}