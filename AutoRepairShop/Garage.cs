using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const int FineForCar = 1000;
        private const int FineForDetail = 100;
        private const int CommandRepair = 0;
        private const int MinCarsPerDay = 1;
        private const int MaxCarsPerDay = 5;

        private Warehouse _warehouse;

        private Queue<Car> _carsQueue;

        internal Garage()
        {
            _carsQueue = new Queue<Car>();
            _warehouse = new Warehouse();
        }

        private List<Detail> DiagnoseFaultyParts(Car car)
        {
            List<Detail> brokenDetails = new List<Detail>();

            foreach (Detail detail in car.GetAllDetails())
            {
                if (detail.IsBroken)
                {
                    brokenDetails.Add(detail);
                }
            }

            return brokenDetails;
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

        private void ReplacementPart(List<Detail> brokenDetails, Detail newDetail, int numberInCollection)
        {
            brokenDetails[numberInCollection] = newDetail;
            brokenDetails.RemoveAt(numberInCollection);
        }

        internal void AddCars()
        {
            for (int i = 0; i < Utils.GetRandomNumber(MinCarsPerDay, MaxCarsPerDay + 1); i++)
            {
                _carsQueue.Enqueue(new Car());
            }
        }

        internal int RepairNextCar(Car car)
        {
            int income = 0;

            if (car != null)
            {
                List<Detail> brokenDetails = DiagnoseFaultyParts(car);

                while (car.CarIsBroken())
                {
                    int userInput = Utils.ReadNumber();

                    if (Utils.ContainsDetailType(userInput))
                    {
                        if ((DetailType)userInput == DetailType.Exit)
                        {
                            income -= FineForDetail * brokenDetails.Count;

                            return income;
                        }

                        Detail newDetail = _warehouse.SendPartToGarage(userInput);

                        for (int i = brokenDetails.Count - 1; i >= 0; i++)
                        {
                            if ((DetailType)userInput == brokenDetails[i].Type)
                            {
                                if (brokenDetails[i].IsBroken)
                                {
                                    ReplacementPart(brokenDetails, newDetail, i);

                                    income += newDetail.Price + newDetail.ReplacementCost;

                                    break;
                                }
                                else
                                {
                                    ReplacementPart(brokenDetails, newDetail, i);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                income -= FineForCar;
            }

            return income;
        }
    }
}
