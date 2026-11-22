using System.Collections.Generic;
using System;

namespace AutoRepairShop
{
    internal class CarsQueue
    {
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private readonly DetailsStore _detailsStore;

        private Queue<Car> _carsQueue;

        internal CarsQueue(DetailsStore detailsStore)
        {
            if (detailsStore == null)
            {
                throw new InvalidOperationException(nameof(detailsStore));
            }

            _carsQueue = new Queue<Car>();
            _detailsStore = detailsStore;
        }

        internal int Count { get; private set; }
        internal bool HasCars
        {
            get
            {
                return (Count > 0);
            }
        }

        internal void AddCars()
        {
            int carsCount = Utils.GetRandomNumber(MinCarsPerDay, MaxCarsPerDay + 1);

            for (int i = Utils.FirstIndex; i < carsCount; i++)
            {
                _carsQueue.Enqueue(new Car(_detailsStore));
            }
        }

        internal Car DequeueCar()
        {
            return _carsQueue.Dequeue();
        }
    }
}