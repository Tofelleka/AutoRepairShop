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
            DetailType.Vaz2108Clutch,
            DetailType.Heater
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
                    "Топливный насос", "Бензонасосы на Самарах перегревались и выходили из строя.", 2_500m, 3_000m),
                new PartTemplate(DetailType.SparkPlugs, PartCategory.Engine,
                    "Свечи зажигания", "Их заливало на карбюраторе или они прогорали.", 1_500m, 800m),
                new PartTemplate(DetailType.Starter, PartCategory.Engine,
                    "Стартер", "Втягивающее реле и сам стартер быстро изнашивались.", 5_000m, 3_500m),
                new PartTemplate(DetailType.RearShockAbsorbers, PartCategory.Engine,
                    "Задние амортизаторы", "Амортизаторы текли и стучали на неровностях.", 1_400m, 1_500m),
                new PartTemplate(DetailType.FrontShockAbsorbers, PartCategory.Engine,
                    "Передние амортизаторы", "Амортизаторы текли и стучали на неровностях.", 1_400m, 1_800m),
                new PartTemplate(DetailType.CoolingRadiator, PartCategory.Engine,
                    "Радиатор охлаждения", "Забивался грязью и начинал течь по стыкам бачков.", 5_000m, 3_000m),
                new PartTemplate(DetailType.Generator, PartCategory.Engine,
                    "Генератор", "Диодный мост и подшипники были слабым местом, часто гудел.", 6_600m, 3_500m),
                new PartTemplate(DetailType.Clutch, PartCategory.Engine,
                    "Сцепление", "Меняли часто: работа дорогая, лезть в коробку сложно.", 5_000m, 8_000m),
                new PartTemplate(DetailType.RearWheelHubBearings, PartCategory.Engine,
                    "Задние ступичные подшипники", "Гудели после 70-75 тыс. км.", 3_200m, 3_000m),
                new PartTemplate(DetailType.FrontWheelHubBearings, PartCategory.Engine,
                    "Передние ступичные подшипники", "Гудели после 50-60 тыс. км.", 3_200m, 4_200m),

                new PartTemplate(DetailType.Hood, PartCategory.Body,
                    "Капот", "После зимы вспучивалась краска и появлялись очаги коррозии.", 14_000m, 3_000m, brand: CarBrand.Vaz),
                new PartTemplate(DetailType.FrontFender, PartCategory.Body,
                    "Переднее крыло", "Первым принимало удары и гнило по нижней кромке.", 2_500m, 2_500m, brand: CarBrand.Vaz),
                new PartTemplate(DetailType.Door, PartCategory.Body,
                    "Дверь", "Петли проседали, низ двери ржавел раньше кузова.", 14_900m, 2_500m, brand: CarBrand.Vaz),
                new PartTemplate(DetailType.Bumper, PartCategory.Body,
                    "Бампер", "Трескался от небольших ударов и терял крепления.", 1_500m, 800m, brand: CarBrand.Vaz),
                new PartTemplate(DetailType.Trunk, PartCategory.Body,
                    "Крышка багажника", "Уплотнитель пропускал воду, металл под ним гнил.", 12_000m, 3_000m, brand: CarBrand.Vaz),
                new PartTemplate(DetailType.Headlight, PartCategory.Body,
                    "Фара", "Стекло мутнело, отражатель выгорал.", 4_300m, 1_200m, brand: CarBrand.Vaz),

                new PartTemplate(DetailType.OxygenSensor, PartCategory.Electric,
                    "Датчик кислорода", "Выходил из строя от плохого топлива, давал ошибку двигателя.", 1_750m, 1_500m),
                new PartTemplate(DetailType.AudioSystem, PartCategory.Electric,
                    "Аудиосистема", "Магнитола хрипела и теряла настройки.", 12_000m, 2_500m),
                new PartTemplate(DetailType.Wiring, PartCategory.Electric,
                    "Проводка", "Окислялась в жгутах и давала короткие замыкания.", 3_000m, 4_000m),
                new PartTemplate(DetailType.Lighting, PartCategory.Electric,
                    "Освещение", "Контакты плавились, свет становился тусклым.", 2_000m, 1_200m),
                new PartTemplate(DetailType.Ecu, PartCategory.Electric,
                    "Электронный блок управления", "Блок сбоил из-за влаги в разъёмах.", 15_000m, 2_500m),
                new PartTemplate(DetailType.Alarm, PartCategory.Electric,
                    "Сигнализация", "Сирена срабатывала сама и сажала аккумулятор.", 5_000m, 3_000m),

                new PartTemplate(DetailType.EngineOil, PartCategory.Fluid,
                    "Моторное масло", "Масло темнело раньше срока и теряло вязкость.", 2_500m, 1_500m, DetailType.Starter),
                new PartTemplate(DetailType.Antifreeze, PartCategory.Fluid,
                    "Антифриз", "Охлаждающая жидкость выгорала и забивала радиатор.", 1_500m, 1_500m, DetailType.CoolingRadiator),
                new PartTemplate(DetailType.BrakeFluid, PartCategory.Fluid,
                    "Тормозная жидкость", "Впитывала влагу, педаль становилась мягкой.", 1_400m, 880m, DetailType.FrontWheelHubBearings),

                new PartTemplate(DetailType.PaintWhite, PartCategory.Material,
                    "Краска белая", "Базовая белая эмаль для полного перекраса.", 1_300m, 100m),
                new PartTemplate(DetailType.PaintBlack, PartCategory.Material,
                    "Краска чёрная", "Глубокий чёрный для полного перекраса.", 1_350m, 100m),
                new PartTemplate(DetailType.PaintRed, PartCategory.Material,
                    "Краска красная", "Насыщенный красный для полного перекраса.", 1_400m, 100m),
                new PartTemplate(DetailType.PaintBlue, PartCategory.Material,
                    "Краска синяя", "Синяя эмаль для полного перекраса.", 1_380m, 100m),
                new PartTemplate(DetailType.Primer, PartCategory.Material,
                    "Грунт", "Обязательный слой перед покраской.", 2_400m, 100m),
                new PartTemplate(DetailType.Lacquer, PartCategory.Material,
                    "Лак", "Защитный слой после краски.", 2_750m, 100m),

                new PartTemplate(DetailType.IzhCarburetor, PartCategory.Engine,
                    "Карбюратор", "Карбюратор переливал, заливало свечи и «трамблёр».", 13_400m, 3_000m,
                    brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhRearAxle, PartCategory.Engine,
                    "Задний мост", "Редуктор моста гудел и сопливился маслом — типичная болезнь Ижей.", 9_000m, 6_000m,
                    brand: CarBrand.Izh, faultChance: 0.30d, extraWork: 2_000m),
                new PartTemplate(DetailType.IzhSteeringRack, PartCategory.Engine,
                    "Рулевая рейка", "Люфт и стуки в рейке — типичная болезнь Ижей.", 6_100m, 4_500m,
                    brand: CarBrand.Izh),

                new PartTemplate(DetailType.VolgaWaterPump, PartCategory.Engine,
                    "Водяной насос", "Помпа текла, подшипник требовал шприцевания.", 2_500m, 1_400m,
                    brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaPowerSteering, PartCategory.Engine,
                    "Гидроусилитель руля", "ГУР подтекал, руль становился тяжёлым.", 12_000m, 2_800m,
                    brand: CarBrand.Volga, faultChance: 0.08d, extraWork: 3_000m),
                new PartTemplate(DetailType.VolgaRearSprings, PartCategory.Engine,
                    "Рессоры", "Рессоры проседали от перегруза, мост подвывал.", 4_600m, 2_000m,
                    brand: CarBrand.Volga, faultChance: 0.18d),

                new PartTemplate(DetailType.Vaz2108Clutch, PartCategory.Engine,
                    "Сцепление «Самара»", "Сцепление и синхронизаторы КПП — слабое место переднеприводных Самар.", 5_000m, 8_000m,
                    models: new[] { "ВАЗ - 2108", "ВАЗ - 2109", "ВАЗ - 2110", "ВАЗ - 2114" }),
                new PartTemplate(DetailType.VazClassicIgnition, PartCategory.Engine,
                    "Трамблёр", "Классическое зажигание — только на заднеприводных ВАЗах.", 4_000m, 1_500m,
                    models: new[] { "ВАЗ - 2101", "ВАЗ - 2103", "ВАЗ - 2104", "ВАЗ - 2105", "ВАЗ - 2106", "ВАЗ - 2107" }),
                new PartTemplate(DetailType.Izh2715BodyPanel, PartCategory.Body,
                    "Борт фургона", "Кузовной элемент только для фургона Иж-2715.", 8_000m, 3_500m,
                    brand: CarBrand.Izh, models: new[] { "Иж - 2715" }),
                new PartTemplate(DetailType.Volga3110Injector, PartCategory.Engine,
                    "Форсунка", "Инжекторная форсунка для ГАЗ-3110.", 3_500m, 2_000m,
                    brand: CarBrand.Volga, models: new[] { "ГАЗ - 3110" }),

                new PartTemplate(DetailType.IzhHood, PartCategory.Body,
                    "Капот Иж", "Капот под свою геометрию — от ВАЗа не встанет.", 13_000m, 3_000m, brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhFender, PartCategory.Body,
                    "Крыло Иж", "Своя штамповка, взаимозаменяемости с ВАЗом нет.", 2_400m, 2_300m, brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhDoor, PartCategory.Body,
                    "Дверь Иж", "Дверь Ижа не подходит к ВАЗу ни по петлям, ни по размеру.", 13_500m, 2_400m, brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhBumper, PartCategory.Body,
                    "Бампер Иж", "Свой бампер под кузов Ижа.", 1_400m, 800m, brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhTrunk, PartCategory.Body,
                    "Крышка багажника Иж", "У фургона и хэтчбека свои крышки.", 11_500m, 2_800m, brand: CarBrand.Izh),
                new PartTemplate(DetailType.IzhHeadlight, PartCategory.Body,
                    "Фара Иж", "Фара под свой кузов, от ВАЗа не подходит.", 4_000m, 1_100m, brand: CarBrand.Izh),

                new PartTemplate(DetailType.VolgaHood, PartCategory.Body,
                    "Капот Волга", "Длинный капот Волги — своя деталь.", 16_000m, 3_500m, brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaFender, PartCategory.Body,
                    "Крыло Волга", "Кузовные панели Волги не взаимозаменяемы с ВАЗом.", 3_000m, 2_800m, brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaDoor, PartCategory.Body,
                    "Дверь Волга", "Тяжёлая дверь Волги со своими петлями.", 17_000m, 3_000m, brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaBumper, PartCategory.Body,
                    "Бампер Волга", "Хромированный бампер, свой по креплениям.", 2_000m, 900m, brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaTrunk, PartCategory.Body,
                    "Крышка багажника Волга", "Своя крышка под большой багажник.", 14_000m, 3_200m, brand: CarBrand.Volga),
                new PartTemplate(DetailType.VolgaHeadlight, PartCategory.Body,
                    "Фара Волга", "Фара с другим отражателем и креплением.", 5_200m, 1_300m, brand: CarBrand.Volga),

                new PartTemplate(DetailType.Heater, PartCategory.Engine,
                    "Печка", "Печка текла и дула холодом — особенно обидно зимой.", 4_500m, 3_500m),

                new PartTemplate(DetailType.TuningChip, PartCategory.Tuning,
                    "Прошивка и настройка", "Настройка зажигания и смеси под модель.", 1_500m, 500m),
                new PartTemplate(DetailType.TuningIntake, PartCategory.Tuning,
                    "Холодный впуск", "Впуск с фильтром нулевого сопротивления.", 8_000m, 2_000m),
                new PartTemplate(DetailType.TuningExhaust, PartCategory.Tuning,
                    "Выпуск 4-2-1", "Прямоточный выпуск для отдачи на верхах.", 12_000m, 3_000m),
                new PartTemplate(DetailType.TuningCamshaft, PartCategory.Tuning,
                    "Распредвал Stage 1", "Спортивный распредвал с разрезной шестернёй.", 25_900m, 6_000m),
                new PartTemplate(DetailType.TuningBrakes, PartCategory.Tuning,
                    "Тормоза улучшенные", "Вентилируемые диски и колодки.", 18_000m, 5_000m),
                new PartTemplate(DetailType.TuningNoise, PartCategory.Tuning,
                    "Шумоизоляция", "Полная шумоизоляция салона.", 30_000m, 8_000m),
                new PartTemplate(DetailType.TuningSuspension, PartCategory.Tuning,
                    "Спортивная подвеска", "Пружины и амортизаторы с занижением.", 22_000m, 6_000m)
            };
        }
    }
}
