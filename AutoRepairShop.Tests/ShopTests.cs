using System.Collections.Generic;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class ShopTests
    {
        [Test]
        public void Buy_SpendsMoneyAndSchedulesDelivery()
        {
            var cash = new CashRegister(100_000m);
            var shop = new PartsShop(cash);
            decimal price = DetailsCatalog.GetTemplate(DetailType.Clutch).GetPrice(DetailsCatalog.StandardBrand);

            PurchaseReport report = shop.Buy(DetailType.Clutch, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.Bought, report.Outcome, "заказ должен пройти");
            Assert.AreEqual(100_000m - price, cash.Money, "деньги списаны сразу");
        }

        [Test]
        public void Buy_PremiumCostsMoreThanEconomy()
        {
            var cash = new CashRegister(1_000_000m);
            var shop = new PartsShop(cash);
            decimal start = cash.Money;

            shop.Buy(DetailType.Starter, DetailsCatalog.Brands[0]);
            decimal economySpent = start - cash.Money;

            start = cash.Money;

            shop.Buy(DetailType.Starter, DetailsCatalog.Brands[2]);
            decimal premiumSpent = start - cash.Money;

            Assert.IsTrue(premiumSpent > economySpent, "премиум должен стоить дороже эконома");
        }

        [Test]
        public void Buy_NotEnoughMoney()
        {
            var cash = new CashRegister(100m);
            var shop = new PartsShop(cash);

            PurchaseReport report = shop.Buy(DetailType.Clutch, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.NotEnoughMoney, report.Outcome, "денег не хватает");
            Assert.AreEqual(100m, cash.Money, "деньги не списаны");
        }

        [Test]
        public void Buy_UnknownType_Refused()
        {
            var cash = new CashRegister(100_000m);
            var shop = new PartsShop(cash);

            PurchaseReport report = shop.Buy((DetailType)99, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.UnknownDetail, report.Outcome, "неизвестный тип отклоняется");
        }

        [Test]
        public void Buy_AllShopPartsHaveValidTemplates()
        {
            foreach (PartTemplate template in DetailsCatalog.Templates)
            {
                Detail detail = new Detail(template, DetailsCatalog.StandardBrand);

                Assert.IsTrue(detail.Price > 0m, "цена должна быть положительной: " + template.Name);
                Assert.IsTrue(detail.ReplacementCost > 0m, "работа должна быть положительной: " + template.Name);
            }
        }

        [Test]
        public void Buy_BodyPartsAreBrandSpecific()
        {
            var cash = new CashRegister(1_000_000m);
            var shop = new PartsShop(cash);

            PurchaseReport report = shop.Buy(DetailType.VolgaDoor, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.Bought, report.Outcome, "дверь Волги заказывается");
        }
    }
}
