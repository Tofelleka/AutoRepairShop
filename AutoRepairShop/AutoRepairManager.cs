using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class AutoRepairManager
    {
        private const int CommandRepair = 0;
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private Queue<Car> _carsQueue;

        internal void AddCars()
        {
            for (int i = 0; i < Utils.GetRandomNumber(MinCarsPerDay, MaxCarsPerDay + 1); i++)
            {
                _carsQueue.Enqueue(new Car());
            }
        }

        private Car GetNextCar()
        {
            Car car = _carsQueue.Dequeue();

            int userInput = Utils.ReadNumber();

            if (userInput == CommandRepair)
            {
                return car;
            }
            else
            {
                return null;
            }
        }
    }
}
