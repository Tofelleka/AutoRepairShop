using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class PartsFactory
    {
        private readonly IReadOnlyDictionary<DetailType, Detail> _catalog;

        public PartsFactory(IReadOnlyDictionary<DetailType, Detail> catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(nameof(catalog));
            }

            _catalog = catalog;
        }

        public Detail Create(DetailType type)
        {
            if (_catalog.TryGetValue(type, out Detail sample) == false)
            {
                throw new ArgumentException($"Неизвестный тип детали: {type}.", nameof(type));
            }

            return new Detail(sample.Type, sample.Description, sample.Name, sample.Price, sample.ReplacementCost);
        }

        public IReadOnlyDictionary<DetailType, Detail> GetAll() => _catalog;
    }
}
