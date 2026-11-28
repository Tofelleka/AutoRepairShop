using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class UpgradeTests
    {
        [Test]
        public static void Catalog_HasFiveUpgrades()
        {
            Assert.AreEqual(5, UpgradeCatalog.All.Count, "в каталоге пять улучшений");
        }

        [Test]
        public static void Buy_IncreasesLevelAndSpends()
        {
            var development = new ServiceDevelopment();
            var cash = new CashRegister(1_000_000m);
            var shop = new UpgradeShop(cash, development);
            decimal price = UpgradeCatalog.Get(UpgradeType.Skill).GetPrice(0);

            UpgradeReport report = shop.Buy(UpgradeType.Skill);

            Assert.AreEqual(UpgradeOutcome.Bought, report.Outcome, "покупка проходит");
            Assert.AreEqual(1, development.GetLevel(UpgradeType.Skill), "уровень вырос");
            Assert.AreEqual(1_000_000m - price, cash.Money, "деньги списаны");
        }

        [Test]
        public static void Buy_MaxLevelReached()
        {
            var development = new ServiceDevelopment();
            var cash = new CashRegister(10_000_000m);
            var shop = new UpgradeShop(cash, development);

            shop.Buy(UpgradeType.Marketing);
            shop.Buy(UpgradeType.Marketing);
            shop.Buy(UpgradeType.Marketing);

            UpgradeReport report = shop.Buy(UpgradeType.Marketing);

            Assert.AreEqual(UpgradeOutcome.MaxLevelReached, report.Outcome, "четвёртая покупка невозможна");
            Assert.AreEqual(3, development.GetLevel(UpgradeType.Marketing), "уровень остался максимальным");
        }

        [Test]
        public static void Boxes_HaveThreeLevels()
        {
            var development = new ServiceDevelopment();
            var cash = new CashRegister(10_000_000m);
            var shop = new UpgradeShop(cash, development);

            shop.Buy(UpgradeType.BodyBox);
            shop.Buy(UpgradeType.BodyBox);
            shop.Buy(UpgradeType.BodyBox);

            UpgradeReport report = shop.Buy(UpgradeType.BodyBox);

            Assert.AreEqual(3, development.GetLevel(UpgradeType.BodyBox), "у бокса три уровня");
            Assert.AreEqual(UpgradeOutcome.MaxLevelReached, report.Outcome, "четвёртый уровень недоступен");
        }

        [Test]
        public static void Buy_NotEnoughMoney()
        {
            var development = new ServiceDevelopment();
            var cash = new CashRegister(100m);
            var shop = new UpgradeShop(cash, development);

            UpgradeReport report = shop.Buy(UpgradeType.Warehouse);

            Assert.AreEqual(UpgradeOutcome.NotEnoughMoney, report.Outcome, "денег не хватает");
            Assert.AreEqual(0, development.GetLevel(UpgradeType.Warehouse), "уровень не изменился");
            Assert.AreEqual(100m, cash.Money, "деньги не списаны");
        }

        [Test]
        public static void Development_RestoresSavedLevels()
        {
            var levels = new System.Collections.Generic.Dictionary<UpgradeType, int>
            {
                { UpgradeType.Warehouse, 2 },
                { UpgradeType.Marketing, 1 },
                { UpgradeType.Skill, 3 },
                { UpgradeType.BodyBox, 1 },
                { UpgradeType.ElectricBox, 0 }
            };

            var development = new ServiceDevelopment(levels);

            Assert.AreEqual(2, development.GetLevel(UpgradeType.Warehouse), "уровень склада восстановлен");
            Assert.AreEqual(1, development.GetLevel(UpgradeType.Marketing), "уровень рекламы восстановлен");
            Assert.AreEqual(3, development.GetLevel(UpgradeType.Skill), "уровень квалификации восстановлен");
            Assert.AreEqual(1, development.GetLevel(UpgradeType.BodyBox), "кузовной бокс восстановлен");
        }
    }
}
