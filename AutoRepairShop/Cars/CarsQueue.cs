using System;
using System.Collections.Generic;
using AutoRepairShop.Common;

namespace AutoRepairShop
{
    internal class CarsQueue
    {
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private readonly IRandomSource _random;
        private readonly Dictionary<DetailType, Detail> _catalog;
        private readonly Queue<Car> _carsQueue;

        public CarsQueue(PartsFactory partsFactory, IRandomSource random)
        {
            if (partsFactory == null)
            {
                throw new ArgumentNullException(nameof(partsFactory));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _random = random;
            _carsQueue = new Queue<Car>();
            _catalog = partsFactory.GetCatalog();
        }

        public int Count
        {
            get
            {
                return _carsQueue.Count;
            }
        }
        public bool HasCars
        {
            get
            {
                return (Count > 0);
            }
        }

        public void AddCars()
        {
            int carsCount = _random.NextInclusive(MinCarsPerDay, MaxCarsPerDay);

            for (int i = 0; i < carsCount; i++)
            {
                _carsQueue.Enqueue(new Car(_catalog, _random));
            }
        }

        public Car DequeueCar()
        {
            return _carsQueue.Dequeue();
        }
    }
}