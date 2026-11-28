using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class TuningTests
    {
        [Test]
        public void FourOptionsPerModel()
        {
            int total = 0;

            foreach (CarBrand brand in new[] { CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga })
            {
                foreach (string model in CarNames.GetNames(brand))
                {
                    int count = TuningCatalog.GetFor(brand, model).Count;
                    Assert.AreEqual(4, count, "у модели " + model + " четыре варианта тюнинга");
                    total += count;
                }
            }

            Assert.AreEqual(64, total, "всего 64 варианта тюнинга");
        }

        [Test]
        public void OptionsHavePricesAndParts()
        {
            foreach (TuningOption option in TuningCatalog.All)
            {
                Assert.IsTrue(option.Price > 0m, "цена тюнинга положительная: " + option.Title);
                Assert.IsTrue(DetailsCatalog.TryGetTemplate(option.PartType, out PartTemplate _),
                    "тюнинг-деталь существует: " + option.Title);
                Assert.IsTrue(option.RequiredBoxLevel >= 1 && option.RequiredBoxLevel <= 3,
                    "уровень бокса в диапазоне: " + option.Title);
            }
        }

        [Test]
        public void TuningBoxIsUpgradeable()
        {
            UpgradeInfo info = UpgradeCatalog.Get(UpgradeType.TuningBox);

            Assert.AreEqual(3, info.MaxLevel, "у тюнинг-бокса три уровня");
            Assert.IsTrue(info.GetPrice(0) > 0m, "первый уровень стоит денег");
        }

        [Test]
        public void CarRemembersTuning()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());
            Car car = factory.Create(CarBrand.Vaz, Season.Summer);

            Assert.IsTrue(car.HasTuning(TuningType.Sport) == false, "сначала тюнинга нет");
            car.AddTuning(TuningType.Sport);
            Assert.IsTrue(car.HasTuning(TuningType.Sport), "тюнинг запоминается");
        }

        [Test]
        public void TuningPartsAreSold()
        {
            Assert.AreEqual(7, DetailsCatalog.GetByCategory(PartCategory.Tuning).Count,
                "семь тюнинг-деталей в магазине");
        }
    }
}
