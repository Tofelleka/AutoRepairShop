using System;
using System.Collections.Generic;
using System.Linq;

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

        public IReadOnlyList<Detail> GetAllDetails() => _details.Values.ToList();

        public bool TryGetDetail(DetailType type, out Detail detail) => _details.TryGetValue(type, out detail);

        public void ReplaceDetail(Detail newDetail) => _details[newDetail.Type] = newDetail;

        public bool CarIsBroken() => _details.Values.Any(detail => detail.IsBroken);
    }
}