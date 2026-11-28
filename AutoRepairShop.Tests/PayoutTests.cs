using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class PayoutTests
    {
        private static Detail CreateDetail(decimal price, decimal work) =>
            new Detail(
                new PartTemplate((DetailType)100, PartCategory.Engine, "Тест", "Тестовая деталь", price, work),
                DetailsCatalog.StandardBrand);

        [Test]
        public static void Payout_NoRate_EqualsPricePlusWork()
        {
            Detail detail = CreateDetail(1000m, 500m);

            Assert.AreEqual(1500m, detail.GetPayout(1m), "без надбавки выплата = цена + работа");
        }

        [Test]
        public static void Payout_RateAppliesToWorkOnly()
        {
            Detail detail = CreateDetail(1000m, 500m);

            Assert.AreEqual(2000m, detail.GetPayout(2m), "удвоение работы не трогает цену детали");
        }

        [Test]
        public static void Payout_RoundsAwayFromZero()
        {
            Detail detail = CreateDetail(1000m, 1155m);

            Assert.AreEqual(2271m, detail.GetPayout(1.1m), "1270,5 округляется до 1271");
        }

        [Test]
        public static void Payout_UniqueBreakdownAddsExtraWork()
        {
            PartTemplate template = DetailsCatalog.GetTemplate(DetailType.IzhRearAxle);
            var detail = new Detail(template, DetailsCatalog.StandardBrand);

            decimal expected = template.GetPrice(DetailsCatalog.StandardBrand)
                + template.GetWork(DetailsCatalog.StandardBrand)
                + template.ExtraWork;

            Assert.IsTrue(template.ExtraWork > 0m, "у моста Ижа есть особая надбавка");
            Assert.AreEqual(expected, detail.GetPayout(1m), "надбавка входит в выплату");
        }

        [Test]
        public static void Payout_BrandChangesPriceAndWork()
        {
            PartTemplate template = DetailsCatalog.GetTemplate(DetailType.Starter);

            var economy = new Detail(template, DetailsCatalog.Brands[0]);
            var premium = new Detail(template, DetailsCatalog.Brands[2]);

            Assert.IsTrue(premium.Price > economy.Price, "премиум дороже");
            Assert.IsTrue(premium.ReplacementCost > economy.ReplacementCost, "работа по премиуму дороже");
        }
    }
}
