namespace AutoRepairShop
{
    internal class PurchaseReport
    {
        public PurchaseReport(PurchaseOutcome outcome, Detail detail)
        {
            Outcome = outcome;
            Detail = detail;
        }

        public PurchaseOutcome Outcome { get; }
        public Detail Detail { get; }
    }
}
