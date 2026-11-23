namespace AutoRepairShop
{
    internal class PurchaseReport
    {
        public PurchaseReport(PurchaseOutcome outcome, Detail detail)
        {
            Outcome = outcome;
            Detail = detail;
        }

        internal PurchaseOutcome Outcome { get; }
        internal Detail Detail { get; }
    }
}
