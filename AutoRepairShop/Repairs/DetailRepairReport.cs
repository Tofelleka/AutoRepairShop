namespace AutoRepairShop
{
    internal class DetailRepairReport
    {
        public DetailRepairReport(RepairOutcome outcome, decimal charge, Detail detail)
        {
            Outcome = outcome;
            Charge = charge;
            Detail = detail;
        }

        public RepairOutcome Outcome { get; }
        public decimal Charge { get; }
        public Detail Detail { get; }
    }
}
