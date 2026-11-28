using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class PaintTests
    {
        [Test]
        public static void PaintColors_HaveFourColors()
        {
            Assert.AreEqual(4, PaintColors.All.Count, "четыре цвета краски");
        }

        [Test]
        public static void PaintColors_MapToTypes()
        {
            Assert.AreEqual(DetailType.PaintWhite, PaintColors.ToDetailType(PaintColor.White), "белая краска");
            Assert.AreEqual(DetailType.PaintBlack, PaintColors.ToDetailType(PaintColor.Black), "чёрная краска");
            Assert.AreEqual(DetailType.PaintRed, PaintColors.ToDetailType(PaintColor.Red), "красная краска");
            Assert.AreEqual(DetailType.PaintBlue, PaintColors.ToDetailType(PaintColor.Blue), "синяя краска");
        }

        [Test]
        public static void Catalog_HasSixMaterials()
        {
            Assert.AreEqual(6, DetailsCatalog.GetByCategory(PartCategory.Material).Count,
                "четыре краски, грунт и лак");
        }

        [Test]
        public static void Car_HasNoMaterials()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            foreach (Detail detail in factory.Create(CarBrand.Vaz).GetAllDetails())
            {
                Assert.IsTrue(detail.Category != PartCategory.Material,
                    "материалы не ставятся на машину: " + detail.Name);
            }
        }
    }
}
