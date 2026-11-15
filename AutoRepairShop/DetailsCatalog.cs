using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class DetailsCatalog
    {
        internal static List<(DetailType type, string name, int price, int replacementCost, string description)> GetDetailsCatalog()
        {
            List<(DetailType type, string name, int price, int replacementCost, string description)> _catalog = new List<(DetailType type, string name, int price, int replacementCost, string description)>
            {
                {(DetailType.FuelPump, "Топливный насос", 2500, 800, "Бензонасосы на Самарах часто перегревались и выходили из строя.") },
                {(DetailType.SparkPlugs, "Свечи зажигания", 600, 200, "Их регулярно заливало (особенно на карбюраторе) или они просто прогорали.") },
                {(DetailType.Starter, "Стартер", 4500, 1200, "Втягивающее реле или сам стартер быстро изнашивались.") },
                {(DetailType.FrontShockAbsorbers, "Передние амортизаторы", 4000, 2000, "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.") },
                {(DetailType.RearShockAbsorbers, "Задние амортизаторы", 4000, 1500, "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.") },
                {(DetailType.CoolingRadiator, "Радиатор охлаждения", 3500, 1500, "Забивался грязью и начинал течь по стыкам бачков.") },
                {(DetailType.Generator, "Генератор", 6000, 1500, "Диодный мост и подшипники были слабым местом, часто гудел.") },
                {(DetailType.Clutch, "Сцепление", 3000, 3500, "Сцепление меняли довольно часто (работа дорогая, лезть в коробку сложно).") },
                {(DetailType.FrontWheelHubBearings, "Передние ступичные подшибники", 2400, 1600, "В передних колесах они гудели после 50-60 тыс. км.") },
                {(DetailType.RearWheelHubBearings, "Задние ступичные подшибники", 2400, 1600, "В задних колесах они гудели после 70-75 тыс. км.") }
            };

            return _catalog;
        }
    }
}