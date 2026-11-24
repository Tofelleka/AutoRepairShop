namespace AutoRepairShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Illustrator illustrator = new Illustrator();
            AutoRepairManager manager = new AutoRepairManager(illustrator);

            manager.Run();
        }
    }
}
