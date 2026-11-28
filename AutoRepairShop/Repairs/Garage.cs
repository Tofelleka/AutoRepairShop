using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const decimal FineForCar = 1000m;
        private const decimal FineForDetail = 100m;

        private readonly Warehouse _warehouse;

        public Garage(Warehouse warehouse)
        {
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            _warehouse = warehouse;
        }

        public List<Detail> DiagnoseFaultyParts(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            return car.GetAllDetails().Where(detail => detail.IsBroken).ToList();
        }

        public DetailRepairReport RepairDetail(DetailType type, Car car)
        {
            if (car.TryGetDetail(type, out Detail replacementDetail) == false)
            {
                throw new ArgumentException($"У машины нет детали типа {type}.", nameof(type));
            }

            if (_warehouse.TryTake(type, out Detail newDetail) == false)
            {
                return new DetailRepairReport(RepairOutcome.NoDetailInStock, 0m, replacementDetail);
            }

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