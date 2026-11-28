using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class CarsQueue
    {
        public const int MinCarsPerDay = 1;
        public const int DefaultMaxCarsPerDay = 5;

        private readonly Queue<Car> _cars = new Queue<Car>();
        private readonly CarFactory _carFactory;
        private readonly IRandomSource _random;

        public CarsQueue(CarFactory carFactory, IRandomSource random)
        {
            if (carFactory == null)
            {
                throw new ArgumentNullException(nameof(carFactory));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _carFactory = carFactory;
            _random = random;
        }

        public int Count => _cars.Count;

        public bool HasCars => _cars.Count > 0;

        public void AddCars(int maxCarsPerDay, IReadOnlyList<CarBrand> brands)
        {
            if (maxCarsPerDay < MinCarsPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCarsPerDay), "В день должно приезжать хотя бы одной машине.");
            }

            if (brands == null || brands.Count == 0)
            {
                throw new ArgumentException("Должна быть хотя бы одна марка машин.", nameof(brands));
            }

            int count = _random.NextInclusive(MinCarsPerDay, maxCarsPerDay);

            for (int i = 0; i < count; i++)
            {
                CarBrand brand = brands[_random.Next(0, brands.Count)];

                _cars.Enqueue(_carFactory.Create(brand));
            }
        }

        public Car DequeueCar()
        {
            if (HasCars == false)
            {
                throw new InvalidOperationException("Очередь машин пуста.");
            }

            return _cars.Dequeue();
        }
    }
}
