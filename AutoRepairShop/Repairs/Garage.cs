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

            var brokenDetails = new List<Detail>();

            foreach (Detail detail in car.GetAllDetails().Values)
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
            var replacementDetail = car.GetAllDetails()[type];

            if (replacementDetail == null)
            {
                throw new ArgumentException($"У машины нет детали типа {type}.", nameof(type));
            }

            if (_warehouse.DetailAvailable(type) == false)
            {
                return new DetailRepairReport(RepairOutcome.NoDetailInStock, 0m, replacementDetail);
            }

            var newDetail = _warehouse.TakeDetail(type);

            car.ReplaceDetail(newDetail);

            if (replacementDetail.IsBroken == false)
            {
                return new DetailRepairReport(RepairOutcome.ReplacedHealthy, 0m, replacementDetail);
            }

            return new DetailRepairReport(RepairOutcome.ReplacedBroken, newDetail.Payout, replacementDetail);
        }
        
        public RepairReport RefuseRepairs() => new RepairReport(0m, FineForCar);

        public RepairReport RefuseFurtherRepairs(decimal revenueFromRepair, Car car) =>
            new RepairReport(revenueFromRepair, FineForDetail * DiagnoseFaultyParts(car).Count);
    }
}