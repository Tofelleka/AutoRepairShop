using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Car
    {
        private readonly Dictionary<DetailType, Detail> _details;

        public Car(string name, Dictionary<DetailType, Detail> details)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название машины не может быть пустым.", nameof(name));
            }

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            _details = details;
            Name = name;
        }

        public string Name { get; }

        public IReadOnlyDictionary<DetailType, Detail> GetAllDetails() => new Dictionary<DetailType, Detail>(_details);

        public bool TryGetDetail(DetailType type, out Detail detail) => _details.TryGetValue(type, out detail);

        public void ReplaceDetail(Detail newDetail) => _details[newDetail.Type] = newDetail;

        public bool CarIsBroken()
        {
            foreach (var pair in _details)
            {
                if (pair.Value.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }
    }
}