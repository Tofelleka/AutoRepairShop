namespace AutoRepairShop.UI
{
    internal interface IRandomSource
    {
        int Next(int minValue, int maxValue);

        bool NextChance(double chance);
    }
}