using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class TuningCatalog
    {
        private static readonly List<TuningOption> Options = CreateOptions();

        public static IReadOnlyList<TuningOption> All => Options;

        public static List<TuningOption> GetFor(CarBrand brand, string model)
        {
            var result = new List<TuningOption>();

            foreach (TuningOption option in Options)
            {
                if (option.Brand == brand && option.Model == model)
                {
                    result.Add(option);
                }
            }

            return result;
        }

        private static List<TuningOption> CreateOptions()
        {
            var list = new List<TuningOption>();

            foreach (CarBrand brand in new[] { CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga })
            {
                foreach (string model in CarNames.GetNames(brand))
                {
                    AddOptions(list, brand, model);
                }
            }

            return list;
        }

        private static void AddOptions(List<TuningOption> list, CarBrand brand, string model)
        {
            decimal factor = GetBrandFactor(brand);

            list.Add(new TuningOption(
                brand,
                model,
                TuningType.Economy,
                "Экономичность",
                EnginePrefix(brand, model) + "настройка смеси и зажигания, ровный холостой. Расход падает на 1–2 литра.",
                Round(12_000m * factor),
                DetailType.TuningChip,
                1));

            list.Add(new TuningOption(
                brand,
                model,
                TuningType.Comfort,
                "Комфорт",
                "Шумоизоляция салона, мягкие опоры двигателя и тише выхлоп. В салоне слышно музыку, а не мост.",
                Round(40_000m * factor),
                DetailType.TuningNoise,
                1));

            list.Add(new TuningOption(
                brand,
                model,
                TuningType.Power,
                "Мощность",
                EnginePrefix(brand, model) + "распредвал Stage 1, доработка ГБЦ, холодный впуск и выпуск 4-2-1. Плюс 15–20 л.с.",
                Round(55_000m * factor),
                DetailType.TuningCamshaft,
                2));

            list.Add(new TuningOption(
                brand,
                model,
                TuningType.Sport,
                "Спорт",
                "Спортивная подвеска и усиленные тормоза: меньше кренов, короче тормозной путь, увереннее в поворотах.",
                Round(85_000m * factor),
                DetailType.TuningSuspension,
                3));
        }

        private static decimal GetBrandFactor(CarBrand brand)
        {
            switch (brand)
            {
                case CarBrand.Volga:
                    return 1.25m;

                case CarBrand.Izh:
                    return 1.1m;

                default:
                    return 1m;
            }
        }

        private static string EnginePrefix(CarBrand brand, string model)
        {
            switch (brand)
            {
                case CarBrand.Izh:
                    return "Мотор на " + model + ": ";

                case CarBrand.Volga:
                    return "ЗМЗ на " + model + ": ";

                default:
                    return model + ": ";
            }
        }

        private static decimal Round(decimal value) => Math.Round(value, 0, MidpointRounding.AwayFromZero);
    }
}
