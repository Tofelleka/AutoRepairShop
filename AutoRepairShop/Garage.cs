using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const decimal FineForCar = 1000m;
        private const decimal FineForDetail = 100m;

        private readonly Warehouse _warehouse;

        internal Garage(Warehouse warehouse)
        {
            _warehouse = warehouse;
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

        internal RepairReport RepairNextCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            decimal revenueFromRepair = 0m;

            List<Detail> brokenDetails = DiagnoseFaultyParts(car);

            while (car.CarIsBroken())
            {
                int userInput = Utils.ReadNumber();

                if (Utils.ContainsDetailType(userInput))
                {
                    DetailType type = (DetailType)userInput;

                    if (type == DetailType.Exit)
                    {
                        return new RepairReport(revenueFromRepair, FineForDetail * brokenDetails.Count);
                    }

                    Detail newDetail = _warehouse.SendPartToGarage(type);

                    if (newDetail != null)
                    {
                        Detail replacementDetail = FindDetailInCar(car, type);

                        if (replacementDetail != null)
                        {
                            if (replacementDetail.IsBroken)
                            {
                                ReplacePart(brokenDetails, newDetail, replacementDetail, car);

                                revenueFromRepair += newDetail.Price + newDetail.ReplacementCost;

                                continue;
                            }
                            else
                            {
                                ReplacePart(brokenDetails, newDetail, replacementDetail, car);
                            }
                        }
                    }
                }
            }

            return new RepairReport(revenueFromRepair, 0m);
        }

        internal RepairReport RefuseCar()
        {
            return new RepairReport(0m, FineForCar);
        }
    }
}