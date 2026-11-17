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

        private void ReplacePart(List<Detail> brokenDetails, Detail newDetail, Detail replacementDetail, Car car)
        {
            car.ReplacementPart(newDetail);

            if (replacementDetail.IsBroken)
            {
                brokenDetails.Remove(replacementDetail);
            }
        }

        private Detail FindDetailInCar(Car car, DetailType type)
        {
            foreach (Detail detail in car.GetAllDetails())
            {
                if (detail.Type == type)
                {
                    return detail;
                }
            }

            return null;
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
                        DetailType type = (DetailType)userInput;

                        if (type == DetailType.Exit)
                        {
                            income -= FineForDetail * brokenDetails.Count;

                            return income;
                        }

                        Detail newDetail = _warehouse.SendPartToGarage(userInput);

                        if (newDetail != null)
                        {
                            Detail replacementDetail = FindDetailInCar(car, type);

                            if (replacementDetail != null)
                            {
                                if (replacementDetail.IsBroken)
                                {
                                    ReplacePart(brokenDetails, newDetail, replacementDetail, car);

                                    income += newDetail.Price + newDetail.ReplacementCost;

                                    break;
                                }
                                else
                                {
                                    ReplacePart(brokenDetails, newDetail, replacementDetail, car);
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
