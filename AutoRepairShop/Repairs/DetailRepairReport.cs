namespace AutoRepairShop
{
    internal class DetailRepairReport
    {
        public DetailRepairReport(
            RepairOutcome outcome,
            decimal charge,
            Detail detail,
            Detail relatedBroken = null)
        {
            Outcome = outcome;
            Charge = charge;
            Detail = detail;
            RelatedBroken = relatedBroken;
        }

        public RepairOutcome Outcome { get; }

        public decimal Charge { get; }

        public Detail Detail { get; }

        public Detail RelatedBroken { get; }
    }
}
