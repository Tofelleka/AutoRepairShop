using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const decimal FineForCar = 1000m;
        private const decimal FineForDetail = 100m;

        private readonly Warehouse _warehouse;

        public Garage(Warehouse warehouse) => _warehouse = warehouse;

        public List<Detail> DiagnoseFaultyParts(Car car)
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

        public DetailRepairReport RepairDetail(DetailType type, Car car)
        {
            var newDetail = _warehouse.SendPartToGarage(type);

            decimal result = 0m;

            var replacementDetail = FindDetailInCar(car, type);

            if (newDetail != null)
            {
                if (replacementDetail != null)
                {
                    if (replacementDetail.IsBroken)
                    {
                        car.ReplaceDetail(newDetail);

                        result = newDetail.Payout;

                        return new DetailRepairReport(RepairOutcome.ReplacedBroken, result, replacementDetail);
                    }
                    else
                    {
                        car.ReplaceDetail(newDetail);

                        return new DetailRepairReport(RepairOutcome.ReplacedHealthy, result, replacementDetail);
                    }
                }
            }

            return new DetailRepairReport(RepairOutcome.NoDetailInStock, result, replacementDetail);
        }

        public RepairReport RefuseRepairs() => new RepairReport(0m, FineForCar);

        public RepairReport RefuseFurtherRepairs(decimal revenueFromRepair, Car car) =>
            new RepairReport(revenueFromRepair, FineForDetail * DiagnoseFaultyParts(car).Count);

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
    }
}