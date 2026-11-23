using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class DetailsCatalog
    {
        internal static Dictionary<DetailType, Detail> GetDetailsCatalog()
        {
            var _catalog = new Dictionary<DetailType, Detail>();

            _catalog[DetailType.FuelPump] = new Detail(DetailType.FuelPump, "Бензонасосы на Самарах часто перегревались и выходили из строя.", "Топливный насос", 2500m, 800m);
            _catalog[DetailType.SparkPlugs] = new Detail(DetailType.SparkPlugs, "Их регулярно заливало (особенно на карбюраторе) или они просто прогорали.", "Свечи зажигания", 600m, 200m);
            _catalog[DetailType.Starter] = new Detail(DetailType.Starter, "Втягивающее реле или сам стартер быстро изнашивались.", "Стартер", 4500m, 1200m);
            _catalog[DetailType.FrontShockAbsorbers] = new Detail(DetailType.FrontShockAbsorbers, "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.", "Передние амортизаторы", 4000m, 2000m);
            _catalog[DetailType.RearShockAbsorbers] = new Detail(DetailType.RearShockAbsorbers, "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.", "Задние амортизаторы", 4000m, 1500m);
            _catalog[DetailType.CoolingRadiator] = new Detail(DetailType.CoolingRadiator, "Забивался грязью и начинал течь по стыкам бачков.", "Радиатор охлаждения", 3500m, 1500m);
            _catalog[DetailType.Generator] = new Detail(DetailType.Generator, "Диодный мост и подшипники были слабым местом, часто гудел.", "Генератор", 6000m, 1500m);
            _catalog[DetailType.Clutch] = new Detail(DetailType.Clutch, "Сцепление меняли довольно часто (работа дорогая, лезть в коробку сложно).", "Сцепление", 3000m, 3500m);
            _catalog[DetailType.FrontWheelHubBearings] = new Detail(DetailType.FrontWheelHubBearings, "В передних колесах они гудели после 50-60 тыс. км.", "Передние ступичные подшипники", 2400m, 1600m);
            _catalog[DetailType.RearWheelHubBearings] = new Detail(DetailType.RearWheelHubBearings, "В задних колесах они гудели после 70-75 тыс. км.", "Задние ступичные подшипники", 2400m, 1600m);

            return _catalog;
        }
    }
}