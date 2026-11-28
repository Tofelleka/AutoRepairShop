namespace AutoRepairShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Illustrator illustrator = new Illustrator();
            int slot = illustrator.AskSaveSlot();
            string savePath = GameSave.GetPath(slot);
            bool isContinue = GameSave.Exists(savePath) && illustrator.AskContinueGame();

            AutoRepairManager manager = new AutoRepairManager(illustrator, slot, isContinue);

            manager.Run();
        }
    }
}
