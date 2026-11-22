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

        internal List<Detail> DiagnoseFaultyParts(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
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

        internal DetailRepairReport RepairDetail(DetailType type, Car car)
        {
            var brokenDetails = DiagnoseFaultyParts(car);

            var newDetail = _warehouse.SendPartToGarage(type);

            decimal result = 0m;

            var replacementDetail = FindDetailInCar(car, type);

            if (newDetail != null)
            {
                if (replacementDetail != null)
                {
                    if (replacementDetail.IsBroken)
                    {
                        ReplacePart(brokenDetails, newDetail, replacementDetail, car);

                        result = newDetail.Price + newDetail.ReplacementCost;

                        return new DetailRepairReport(RepairOutcome.ReplacedBroken, result, replacementDetail);
                    }
                    else
                    {
                        ReplacePart(brokenDetails, newDetail, replacementDetail, car);

                        return new DetailRepairReport(RepairOutcome.ReplacedHealthe, result, replacementDetail);
                    }
                }
            }

            return new DetailRepairReport(RepairOutcome.NoDetailInStock, result, replacementDetail);
        }

        internal RepairReport RefuseRepairs()
        {
            return new RepairReport(0m, FineForCar);
        }

        internal RepairReport RefuseFurtherRepairs(decimal revenueFromRepair, Car car)
        {
            return new RepairReport(revenueFromRepair, FineForDetail * DiagnoseFaultyParts(car).Count);
        }
    }
}