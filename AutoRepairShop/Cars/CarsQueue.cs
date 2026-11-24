using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class CarsQueue
    {
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private readonly CarFactory _carFactory;
        private readonly IRandomSource _random;
        private readonly Queue<Car> _carsQueue;

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
            _carsQueue = new Queue<Car>();
        }

        public int Count => _carsQueue.Count;

        public bool HasCars => Count > 0;

        public void AddCars()
        {
            int carsCount = _random.NextInclusive(MinCarsPerDay, MaxCarsPerDay);

            for (int i = 0; i < carsCount; i++)
            {
                _carsQueue.Enqueue(_carFactory.Create());
            }
        }

        public Car DequeueCar() => _carsQueue.Dequeue();
    }
}
