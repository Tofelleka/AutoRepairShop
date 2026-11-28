using System;

namespace AutoRepairShop
{
    internal class Detail
    {
        private const decimal RepairWorkFactor = 0.6m;
        private const decimal PartsMarkup = 1.1m;

        public Detail(PartTemplate template, Brand brand)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            Type = template.Type;
            Category = template.Category;
            Name = template.Name;
            Description = template.Description;
            BrandName = brand.Name;
            FaultFactor = brand.FaultFactor;
            Price = template.GetPrice(brand);
            ReplacementCost = template.GetWork(brand);
            ExtraWork = template.ExtraWork;
        }

        public DetailType Type { get; }

        public PartCategory Category { get; }

        public string Name { get; }

        public string Description { get; }

        public string BrandName { get; }

        public double FaultFactor { get; }

        public decimal Price { get; }

        public decimal ReplacementCost { get; }

        public bool IsBroken { get; private set; }

        public bool IsRepairable { get; private set; }

        public decimal ExtraWork { get; }

        public decimal Payout => GetPayout(1m);

        public decimal GetPayout(decimal workRate)
        {
            if (workRate < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(workRate), "Коэффициент работы не может быть отрицательным.");
            }

            decimal work = Math.Round(ReplacementCost * workRate, 0, MidpointRounding.AwayFromZero);
            decimal part = Math.Round(Price * PartsMarkup, 0, MidpointRounding.AwayFromZero);

            return part + work + ExtraWork;
        }

        public decimal GetRepairCharge(decimal workRate)
        {
            if (workRate < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(workRate), "Коэффициент работы не может быть отрицательным.");
            }

            decimal work = Math.Round(ReplacementCost * RepairWorkFactor * workRate, 0, MidpointRounding.AwayFromZero);

            return work + ExtraWork;
        }

        public void BrokeDetail() => IsBroken = true;

        public void MarkRepairable() => IsRepairable = true;

        public void Repair()
        {
            IsBroken = false;
            IsRepairable = false;
        }

        public void RestoreState(bool isBroken, bool isRepairable)
        {
            IsBroken = isBroken;

            if (isBroken && isRepairable)
            {
                IsRepairable = true;
            }
        }
    }
}
