using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class DetailsCatalog
    {
        private static readonly List<PartTemplate> PartTemplates = CreateTemplates();
        private static readonly List<Brand> PartBrands = CreateBrands();

        private static readonly HashSet<DetailType> RepairableTypes = new HashSet<DetailType>
        {
            DetailType.CoolingRadiator,
            DetailType.Starter,
            DetailType.Generator,
            DetailType.Wiring,
            DetailType.Door,
            DetailType.FrontFender,
            DetailType.IzhSteeringRack,
            DetailType.VolgaWaterPump,
            DetailType.Vaz2108Clutch
        };

        public static IReadOnlyList<PartTemplate> Templates => PartTemplates;

        public static IReadOnlyList<Brand> Brands => PartBrands;

        public static Brand StandardBrand => PartBrands[1];

        public static PartTemplate GetTemplate(DetailType type)
        {
            if (TryGetTemplate(type, out PartTemplate template) == false)
            {
                throw new ArgumentException($"Неизвестный тип детали: {type}.", nameof(type));
            }

            return template;
        }

        public static bool TryGetTemplate(DetailType type, out PartTemplate template)
        {
            foreach (PartTemplate item in PartTemplates)
            {
                if (item.Type == type)
                {
                    template = item;

                    return true;
                }
            }

            template = null;

            return false;
        }

        public static PartCategory GetCategory(DetailType type) => GetTemplate(type).Category;

        public static List<PartTemplate> GetTemplatesFor(CarBrand brand, string model)
        {
            var result = new List<PartTemplate>();

            foreach (PartTemplate item in PartTemplates)
            {
                if (item.Brand.HasValue && item.Brand.Value != brand)
                {
                    continue;
                }

                if (item.FitsModel(model) == false)
                {
                    continue;
                }

                result.Add(item);
            }

            return result;
        }

        public static bool CanRepair(DetailType type) => RepairableTypes.Contains(type);

        public static List<PartTemplate> GetTemplatesFor(CarBrand brand)
        {
            var result = new List<PartTemplate>();

            foreach (PartTemplate item in PartTemplates)
            {
                if (item.Brand.HasValue == false || item.Brand.Value == brand)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static List<PartTemplate> GetByCategory(PartCategory category)
        {
            var result = new List<PartTemplate>();

            foreach (PartTemplate item in PartTemplates)
            {
                if (item.Category == category)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private static List<Brand> CreateBrands()
        {
            return new List<Brand>
            {
                new Brand("NoName", 0.8m, 0.8m, 1.3d),
                new Brand("АвтоВАЗ", 1m, 1m, 1d),
                new Brand("Bosch", 1.4m, 1.4m, 0.7d)
            };
        }

        private static List<PartTemplate> CreateTemplates()
        {
            return new List<PartTemplate>
            {
                new PartTemplate(DetailType.FuelPump, PartCategory.Engine,
                    "Топливный насос", "Бензонасосы на Самарах часто перегревались и выходили из строя.", 6_000m, 2_500m),
                new PartTemplate(DetailType.SparkPlugs, PartCategory.Engine,
                    "Свечи зажигания", "Их регулярно заливало (особенно на карбюраторе) или они просто прогорали.", 1_500m, 600m),
                new PartTemplate(DetailType.Starter, PartCategory.Engine,
                    "Стартер", "Втягивающее реле или сам стартер быстро изнашивались.", 11_000m, 4_000m),
                new PartTemplate(DetailType.RearShockAbsorbers, PartCategory.Engine,
                    "Задние амортизаторы", "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.", 10_000m, 5_000m),
                new PartTemplate(DetailType.FrontShockAbsorbers, PartCategory.Engine,
                    "Передние амортизаторы", "Подвеска Самар славилась своей «энергоемкостью», амортизаторы текли.", 10_000m, 6_000m),
                new PartTemplate(DetailType.CoolingRadiator, PartCategory.Engine,
                    "Радиатор охлаждения", "Забивался грязью и начинал течь по стыкам бачков.", 9_000m, 5_000m),
                new PartTemplate(DetailType.Generator, PartCategory.Engine,
                    "Генератор", "Диодный мост и подшипники были слабым местом, часто гудел.", 15_000m, 5_000m),
                new PartTemplate(DetailType.Clutch, PartCategory.Engine,
                    "Сцепление", "Сцепление меняли довольно часто (работа дорогая, лезть в коробку сложно).", 9_000m, 8_000m),
                new PartTemplate(DetailType.RearWheelHubBearings, PartCategory.Engine,
                    "Задние ступичные подшипники", "В задних колесах они гудели после 70-75 тыс. км.", 6_000m, 5_000m),
                new PartTemplate(DetailType.FrontWheelHubBearings, PartCategory.Engine,
                    "Передние ступичные подшипники", "В передних колесах они гудели после 50-60 тыс. км.", 6_000m, 5_000m),

                new PartTemplate(DetailType.Hood, PartCategory.Body,
                    "Капот", "После зимы вспучивалась краска и появлялись очаги коррозии.", 12_000m, 6_000m),
                new PartTemplate(DetailType.FrontFender, PartCategory.Body,
                    "Переднее крыло", "Первым принимало на себя удары и гнило по нижней кромке.", 9_000m, 5_500m),
                new PartTemplate(DetailType.Door, PartCategory.Body,
                    "Дверь", "Петли проседали, а низ двери ржавел раньше остального кузова.", 14_000m, 7_000m),
                new PartTemplate(DetailType.Bumper, PartCategory.Body,
                    "Бампер", "Трескался от небольших ударов и терял крепления.", 8_000m, 5_000m),
                new PartTemplate(DetailType.Trunk, PartCategory.Body,
                    "Крышка багажника", "Уплотнитель пропускал воду, и металл под ним гнил.", 10_000m, 6_000m),
                new PartTemplate(DetailType.Headlight, PartCategory.Body,
                    "Фара", "Стекло мутнело, а отражатель выгорал от времени.", 5_000m, 3_000m),

                new PartTemplate(DetailType.OxygenSensor, PartCategory.Electric,
                    "Датчик кислорода", "Выходил из строя от плохого топлива и давал ошибку двигателя.", 4_000m, 2_500m),
                new PartTemplate(DetailType.AudioSystem, PartCategory.Electric,
                    "Аудиосистема", "Штатная магнитола хрипела и теряла настройки.", 7_000m, 4_000m),
                new PartTemplate(DetailType.Wiring, PartCategory.Electric,
                    "Проводка", "Окислялась в жгутах и давала короткие замыкания.", 6_000m, 5_000m),
                new PartTemplate(DetailType.Lighting, PartCategory.Electric,
                    "Освещение", "Контакты плавились, свет становился тусклым.", 5_000m, 3_000m),
                new PartTemplate(DetailType.Ecu, PartCategory.Electric,
                    "Электронный блок управления", "Блок управления сбоил из-за влаги в разъёмах.", 15_000m, 6_000m),
                new PartTemplate(DetailType.Alarm, PartCategory.Electric,
                    "Сигнализация", "Сирена срабатывала сама и сажала аккумулятор.", 6_000m, 3_500m),

                new PartTemplate(DetailType.EngineOil, PartCategory.Fluid,
                    "Моторное масло", "Масло темнело раньше срока и теряло вязкость.", 2_000m, 800m, DetailType.Starter),
                new PartTemplate(DetailType.Antifreeze, PartCategory.Fluid,
                    "Антифриз", "Охлаждающая жидкость выгорала и забивала радиатор.", 1_200m, 600m, DetailType.CoolingRadiator),
                new PartTemplate(DetailType.BrakeFluid, PartCategory.Fluid,
                    "Тормозная жидкость", "Впитывала влагу, и педаль становилась мягкой.", 1_500m, 700m, DetailType.FrontWheelHubBearings),

                new PartTemplate(DetailType.PaintWhite, PartCategory.Material,
                    "Краска белая", "Базовая белая эмаль для полного перекраса.", 3_000m, 100m),
                new PartTemplate(DetailType.PaintBlack, PartCategory.Material,
                    "Краска чёрная", "Глубокий чёрный для полного перекраса.", 3_200m, 100m),
                new PartTemplate(DetailType.PaintRed, PartCategory.Material,
                    "Краска красная", "Насыщенный красный для полного перекраса.", 3_400m, 100m),
                new PartTemplate(DetailType.PaintBlue, PartCategory.Material,
                    "Краска синяя", "Синяя эмаль для полного перекраса.", 3_300m, 100m),
                new PartTemplate(DetailType.Primer, PartCategory.Material,
                    "Грунт", "Обязательный слой перед покраской.", 1_500m, 100m),
                new PartTemplate(DetailType.Lacquer, PartCategory.Material,
                    "Лак", "Защитный слой после краски.", 2_000m, 100m),

                new PartTemplate(DetailType.IzhCarburetor, PartCategory.Engine,
                    "Карбюратор", "«Солекс» на Ижах капризничал и переливал топливо.", 5_000m, 2_500m,
                    brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhRearAxle, PartCategory.Engine,
                    "Задний мост", "Полуоси и редуктор Ижа гудели после долгой езды.", 12_000m, 7_000m,
                    brand: CarBrand.Izh, faultChance: 0.30d, extraWork: 2_000m),
                new PartTemplate(DetailType.IzhSteeringRack, PartCategory.Engine,
                    "Рулевая рейка", "Люфт и стуки в рейке — типичная болезнь Ижей.", 9_000m, 6_000m,
                    brand: CarBrand.Izh),

                new PartTemplate(DetailType.VolgaWaterPump, PartCategory.Engine,
                    "Водяной насос", "Помпа Волги начинала течь и перегревала мотор.", 7_000m, 4_000m,
                    brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaPowerSteering, PartCategory.Engine,
                    "Гидроусилитель руля", "ГУР подтекал и руль становился тяжёлым.", 18_000m, 9_000m,
                    brand: CarBrand.Volga, faultChance: 0.08d, extraWork: 3_000m),
                new PartTemplate(DetailType.VolgaRearSprings, PartCategory.Engine,
                    "Рессоры", "Листы рессор лопались от перегруза.", 11_000m, 6_000m,
                    brand: CarBrand.Volga),

                new PartTemplate(DetailType.Vaz2108Clutch, PartCategory.Engine,
                    "Сцепление «Самара»", "Стоит только на переднеприводных Самарах.", 9_000m, 5_500m,
                    models: new[] { "ВАЗ - 2108", "ВАЗ - 2109", "ВАЗ - 2110", "ВАЗ - 2114" }),
                new PartTemplate(DetailType.VazClassicIgnition, PartCategory.Engine,
                    "Трамблёр", "Классическое зажигание — только на заднеприводных ВАЗах.", 4_000m, 2_000m,
                    models: new[] { "ВАЗ - 2101", "ВАЗ - 2103", "ВАЗ - 2104", "ВАЗ - 2105", "ВАЗ - 2106", "ВАЗ - 2107" }),
                new PartTemplate(DetailType.Izh2715BodyPanel, PartCategory.Body,
                    "Борт фургона", "Кузовной элемент только для фургона Иж-2715.", 7_000m, 4_000m,
                    brand: CarBrand.Izh, models: new[] { "Иж - 2715" }),
                new PartTemplate(DetailType.Volga3110Injector, PartCategory.Engine,
                    "Форсунка", "Инжекторная форсунка для ГАЗ-3110.", 6_000m, 3_500m,
                    brand: CarBrand.Volga, models: new[] { "ГАЗ - 3110" })
            };
        }
    }
}
