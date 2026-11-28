namespace AutoRepairShop
{
    internal class DayStatistics
    {
        public DayStatistics(int cars, int refused, int repaired, decimal fines, decimal revenue)
        {
            Cars = cars;
            Refused = refused;
            Repaired = repaired;
            Fines = fines;
            Revenue = revenue;
        }

        public int Cars { get; }
        public int Refused { get; }
        public int Repaired { get; }
        public decimal Fines { get; }
        public decimal Revenue { get; }
        public decimal Profit => Revenue - Fines;
    }
}
