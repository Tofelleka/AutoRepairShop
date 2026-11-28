using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class CarNames
    {
        private static readonly List<string> VazNames = new List<string>
        {
            "ВАЗ - 2101",
            "ВАЗ - 2103",
            "ВАЗ - 2104",
            "ВАЗ - 2105",
            "ВАЗ - 2106",
            "ВАЗ - 2107",
            "ВАЗ - 2108",
            "ВАЗ - 2109",
            "ВАЗ - 2110",
            "ВАЗ - 2114"
        };

        private static readonly List<string> IzhNames = new List<string>
        {
            "Иж - 2126",
            "Иж - 2715",
            "Иж - 2125"
        };

        private static readonly List<string> VolgaNames = new List<string>
        {
            "ГАЗ - 24",
            "ГАЗ - 3102",
            "ГАЗ - 3110"
        };

        public static IReadOnlyList<string> GetNames(CarBrand brand)
        {
            switch (brand)
            {
                case CarBrand.Izh:
                    return IzhNames;

                case CarBrand.Volga:
                    return VolgaNames;

                default:
                    return VazNames;
            }
        }

        public static string GetBrandTitle(CarBrand brand)
        {
            switch (brand)
            {
                case CarBrand.Izh:
                    return "Иж";

                case CarBrand.Volga:
                    return "Волга";

                default:
                    return "ВАЗ";
            }
        }
    }
}
