using System.Collections.Generic;
using System;
using AutoRepairShop.UI;

namespace AutoRepairShop
{
    internal class CarsQueue
    {
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private readonly IRandomSource _random;
        private readonly Dictionary<DetailType, Detail> _catalog;
        private readonly Queue<Car> _carsQueue;

        internal CarsQueue(PartsFactory detailsStore, IRandomSource random)
        {
            if (detailsStore == null)
            {
                throw new ArgumentNullException(nameof(detailsStore));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _random = random;
            _carsQueue = new Queue<Car>();
            _catalog = detailsStore.GetCatalog();
        }

        internal int Count
        {
            get
            {
                return _carsQueue.Count;
            }
        }
        internal bool HasCars
        {
            get
            {
                return (Count > 0);
            }
        }

        internal void AddCars()
        {
            int carsCount = _random.Next(MinCarsPerDay, MaxCarsPerDay + 1);

            for (int i = 0; i < carsCount; i++)
            {
                _carsQueue.Enqueue(new Car(_catalog, _random));
            }
        }

        internal Car DequeueCar()
        {
            return _carsQueue.Dequeue();
        }
    }
}