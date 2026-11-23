namespace AutoRepairShop
{
    internal class DetailRepairReport
    {
        internal DetailRepairReport(RepairOutcome outcome, decimal charge, Detail detail)
        {
            Outcome = outcome;
            Charge = charge;
            Detail = detail;
        }

        internal RepairOutcome Outcome { get; }
        internal decimal Charge { get; }
        internal Detail Detail { get; }
    }
}
