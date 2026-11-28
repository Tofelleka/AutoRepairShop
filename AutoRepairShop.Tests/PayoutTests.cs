using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class PayoutTests
    {
        private static Detail CreateDetail(decimal price, decimal work) =>
            new Detail(
                new PartTemplate((DetailType)100, PartCategory.Engine, "Тест", "Тестовая деталь", price, work),
                DetailsCatalog.StandardBrand);

        [Test]
        public void Payout_IncludesPartsMarkup()
        {
            Detail detail = CreateDetail(1000m, 500m);

            Assert.AreEqual(1600m, detail.GetPayout(1m), "1000 + 10% наценки + 500 работы");
        }

        [Test]
        public void Payout_RateAppliesToWorkOnly()
        {
            Detail detail = CreateDetail(1000m, 500m);

            Assert.AreEqual(2100m, detail.GetPayout(2m), "удвоение работы не трогает цену детали");
        }

        [Test]
        public void Payout_RoundsAwayFromZero()
        {
            Detail detail = CreateDetail(1000m, 1155m);

            Assert.AreEqual(2371m, detail.GetPayout(1.1m), "1100 + 1271");
        }

        [Test]
        public void Payout_UniqueBreakdownAddsExtraWork()
        {
            PartTemplate template = DetailsCatalog.GetTemplate(DetailType.IzhRearAxle);
            var detail = new Detail(template, DetailsCatalog.StandardBrand);

            decimal expected = detail.GetPayout(1m) - template.ExtraWork + template.ExtraWork;

            Assert.IsTrue(template.ExtraWork > 0m, "у моста Ижа есть особая надбавка");
            Assert.AreEqual(expected, detail.GetPayout(1m), "надбавка входит в выплату");
        }

        [Test]
        public void Payout_BrandChangesPriceAndWork()
        {
            PartTemplate template = DetailsCatalog.GetTemplate(DetailType.Starter);

            var economy = new Detail(template, DetailsCatalog.Brands[0]);
            var premium = new Detail(template, DetailsCatalog.Brands[2]);

            Assert.IsTrue(premium.Price > economy.Price, "премиум дороже");
            Assert.IsTrue(premium.ReplacementCost > economy.ReplacementCost, "работа по премиуму дороже");
        }
    }
}
