using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const int FineForCar = 1000;
        private const int FineForDetail = 100;

        private Warehouse _warehouse;

        private List<Detail> DiagnoseFaultyParts(Car car)
        {
            if(car == null) 
            {
                throw new InvalidOperationException(nameof(car));
            }

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

                        Detail newDetail = _warehouse.SendPartToGarage(type);

                        if (newDetail != null)
                        {
                            Detail replacementDetail = FindDetailInCar(car, type);

                            if (replacementDetail != null)
                            {
                                if (replacementDetail.IsBroken)
                                {
                                    ReplacePart(brokenDetails, newDetail, replacementDetail, car);

                                    income += newDetail.Price + newDetail.ReplacementCost;

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
            }
            else
            {
                income -= FineForCar;
            }

            return income;
        }
    }
}
