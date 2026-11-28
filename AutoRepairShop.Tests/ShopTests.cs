using System.Collections.Generic;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class ShopTests
    {
        [Test]
        public static void Buy_SpendsMoneyAndAddsStock()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            var cash = new CashRegister(100_000m);
            var shop = new PartsShop(warehouse, cash);
            int before = warehouse.GetQuantityDetail(DetailType.Clutch);
            decimal price = DetailsCatalog.GetTemplate(DetailType.Clutch).GetPrice(DetailsCatalog.StandardBrand);

            PurchaseReport report = shop.Buy(DetailType.Clutch, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.Bought, report.Outcome, "покупка должна пройти");
            Assert.AreEqual(100_000m - price, cash.Money, "деньги списаны");
            Assert.AreEqual(before + 1, warehouse.GetQuantityDetail(DetailType.Clutch), "деталь на складе");
        }

        [Test]
        public static void Buy_PremiumCostsMoreThanEconomy()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            var cash = new CashRegister(1_000_000m);
            var shop = new PartsShop(warehouse, cash);
            decimal start = cash.Money;

            shop.Buy(DetailType.Starter, DetailsCatalog.Brands[0]);
            decimal economySpent = start - cash.Money;

            start = cash.Money;

            shop.Buy(DetailType.Starter, DetailsCatalog.Brands[2]);
            decimal premiumSpent = start - cash.Money;

            Assert.IsTrue(premiumSpent > economySpent, "премиум должен стоить дороже эконома");
        }

        [Test]
        public static void Buy_NotEnoughMoney()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            var cash = new CashRegister(100m);
            var shop = new PartsShop(warehouse, cash);
            int before = warehouse.GetQuantityDetail(DetailType.Clutch);

            PurchaseReport report = shop.Buy(DetailType.Clutch, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.NotEnoughMoney, report.Outcome, "денег не хватает");
            Assert.AreEqual(100m, cash.Money, "деньги не списаны");
            Assert.AreEqual(before, warehouse.GetQuantityDetail(DetailType.Clutch), "склад не изменился");
        }

        [Test]
        public static void Buy_WarehouseIsFull()
        {
            var quantities = new Dictionary<DetailType, int> { { DetailType.Starter, 1 } };
            Warehouse warehouse = new WarehouseFactory(new PartsFactory()).Create(1, quantities);
            var cash = new CashRegister(1_000_000m);
            var shop = new PartsShop(warehouse, cash);

            PurchaseReport report = shop.Buy(DetailType.Generator, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.WarehouseIsFull, report.Outcome, "склад заполнен");
            Assert.AreEqual(1_000_000m, cash.Money, "деньги не списаны");
            Assert.AreEqual(1, warehouse.TotalQuantity, "запас не вырос");
        }

        [Test]
        public static void Buy_UnknownType_Refused()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            var cash = new CashRegister(100_000m);
            var shop = new PartsShop(warehouse, cash);

            PurchaseReport report = shop.Buy((DetailType)99, DetailsCatalog.StandardBrand);

            Assert.AreEqual(PurchaseOutcome.UnknownDetail, report.Outcome, "неизвестный тип отклоняется");
        }
    }
}
