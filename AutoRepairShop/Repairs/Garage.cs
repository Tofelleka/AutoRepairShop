using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairShop
{
    internal class Garage
    {
        private const decimal FineForCar = 2_000m;
        private const decimal FineForDetail = 300m;
        private const double CheapFluidRiskChance = 0.35d;

        private readonly Warehouse _warehouse;
        private readonly IRandomSource _random;

        public Garage(Warehouse warehouse, IRandomSource random)
        {
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            _warehouse = warehouse;
            _random = random;
        }

        public List<Detail> DiagnoseFaultyParts(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            return car.GetAllDetails().Where(detail => detail.IsBroken).ToList();
        }

        public DetailRepairReport RepairDetail(DetailType type, Car car, decimal workRate)
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

            Detail relatedBroken = TryBreakRelatedDetail(car, newDetail);

            if (replacementDetail.IsBroken == false)
            {
                return new DetailRepairReport(RepairOutcome.ReplacedHealthy, 0m, replacementDetail, relatedBroken);
            }

            return new DetailRepairReport(
                RepairOutcome.ReplacedBroken,
                newDetail.GetPayout(workRate),
                replacementDetail,
                relatedBroken);
        }

        public DetailRepairReport RepairWithoutReplacement(DetailType type, Car car, decimal workRate)
        {
            if (car.TryGetDetail(type, out Detail detail) == false)
            {
                throw new ArgumentException($"У машины нет детали типа {type}.", nameof(type));
            }

            if (detail.IsBroken == false || detail.IsRepairable == false)
            {
                return new DetailRepairReport(RepairOutcome.NotRepairable, 0m, detail);
            }

            decimal charge = detail.GetRepairCharge(workRate);

            detail.Repair();

            return new DetailRepairReport(RepairOutcome.Repaired, charge, detail);
        }

        public RepairReport RefuseRepairs() => new RepairReport(0m, FineForCar);
        public RepairReport RefuseFurtherRepairs(decimal revenueFromRepair, Car car) =>
            new RepairReport(revenueFromRepair, FineForDetail * DiagnoseFaultyParts(car).Count);

        private Detail TryBreakRelatedDetail(Car car, Detail installedDetail)
        {
            if (installedDetail.Category != PartCategory.Fluid)
            {
                return null;
            }

            if (installedDetail.FaultFactor <= 1d)
            {
                return null;
            }

            PartTemplate template = DetailsCatalog.GetTemplate(installedDetail.Type);

            if (template.RelatedType.HasValue == false)
            {
                return null;
            }

            if (_random.NextChance(CheapFluidRiskChance) == false)
            {
                return null;
            }

            if (car.TryGetDetail(template.RelatedType.Value, out Detail related) == false)
            {
                return null;
            }

            if (related.IsBroken)
            {
                return null;
            }

            related.BrokeDetail();

            return related;
        }
    }
}
