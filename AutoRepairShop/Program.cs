namespace AutoRepairShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AutoRepairManager manager = new AutoRepairManager();

            manager.SimulateWorkDay();
        }
    }
}