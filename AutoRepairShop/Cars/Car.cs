using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
        private readonly Dictionary<DetailType, Detail> _details;

        public Car(CarBrand brand, string name, IReadOnlyDictionary<DetailType, Detail> details)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название машины не может быть пустым.", nameof(name));
            }

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            Brand = brand;
            Name = name;
            _details = new Dictionary<DetailType, Detail>(details.Count);

            foreach (KeyValuePair<DetailType, Detail> pair in details)
            {
                _details[pair.Key] = pair.Value;
            }
        }

        public CarBrand Brand { get; }

        public string Name { get; }

        public IReadOnlyList<Detail> GetAllDetails() => new List<Detail>(_details.Values);

        public bool CarIsBroken()
        {
            foreach (Detail detail in _details.Values)
            {
                if (detail.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetDetail(DetailType type, out Detail detail) => _details.TryGetValue(type, out detail);

        public void ReplaceDetail(Detail detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException(nameof(detail));
            }

            _details[detail.Type] = detail;
        }
    }
}
