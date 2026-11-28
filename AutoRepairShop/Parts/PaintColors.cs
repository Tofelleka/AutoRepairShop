using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class PaintColors
    {
        private static readonly List<PaintColor> ColorList = new List<PaintColor>
        {
            PaintColor.White,
            PaintColor.Black,
            PaintColor.Red,
            PaintColor.Blue
        };

        public static IReadOnlyList<PaintColor> All => ColorList;

        public static DetailType ToDetailType(PaintColor color)
        {
            switch (color)
            {
                case PaintColor.Black:
                    return DetailType.PaintBlack;

                case PaintColor.Red:
                    return DetailType.PaintRed;

                case PaintColor.Blue:
                    return DetailType.PaintBlue;

                default:
                    return DetailType.PaintWhite;
            }
        }

        public static string GetName(PaintColor color)
        {
            switch (color)
            {
                case PaintColor.Black:
                    return "чёрный";

                case PaintColor.Red:
                    return "красный";

                case PaintColor.Blue:
                    return "синий";

                default:
                    return "белый";
            }
        }
    }
}
