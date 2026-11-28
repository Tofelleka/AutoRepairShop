using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class WorkRatesTests
    {
        [Test]
        public static void BaseRateIsOne()
        {
            var rates = new WorkRates(0m, 0m, 0m);

            Assert.AreEqual(1m, rates.GetRate(PartCategory.Engine), "без прокачки коэффициент 1.0");
            Assert.AreEqual(1m, rates.GetRate(PartCategory.Fluid), "жидкости тоже 1.0");
        }

        [Test]
        public static void BodyBonusAppliesOnlyToBody()
        {
            var rates = new WorkRates(0m, 0.1m, 0.2m);

            Assert.AreEqual(1.1m, rates.GetRate(PartCategory.Body), "кузов получает свой бонус");
            Assert.AreEqual(1.2m, rates.GetRate(PartCategory.Electric), "электрика получает свой");
            Assert.AreEqual(1m, rates.GetRate(PartCategory.Engine), "двигатель без бонусов");
        }

        [Test]
        public static void SkillAppliesEverywhere()
        {
            var rates = new WorkRates(0.1m, 0.1m, 0.1m);

            Assert.AreEqual(1.1m, rates.GetRate(PartCategory.Engine), "общая квалификация действует везде");
            Assert.AreEqual(1.2m, rates.GetRate(PartCategory.Body), "в кузове складывается с бонусом бокса");
        }
    }
}
