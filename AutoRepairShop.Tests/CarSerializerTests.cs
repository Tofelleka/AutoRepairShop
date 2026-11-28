using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class CarSerializerTests
    {
        [Test]
        public void RoundTrip_KeepsCarState()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());
            Car original = factory.Create(CarBrand.Izh, Season.Autumn);

            original.NeedTireChange();
            original.AddTuning(TuningType.Comfort);

            string text = CarSerializer.Serialize(original);
            Car restored = CarSerializer.Deserialize(text);

            Assert.AreEqual(original.Brand, restored.Brand, "марка сохранена");
            Assert.AreEqual(original.Name, restored.Name, "модель сохранена");
            Assert.AreEqual(original.NeedsTireChange, restored.NeedsTireChange, "шиномонтаж сохранён");
            Assert.IsTrue(restored.HasTuning(TuningType.Comfort), "тюнинг сохранён");
            Assert.AreEqual(original.GetAllDetails().Count, restored.GetAllDetails().Count, "все детали на месте");
        }

        [Test]
        public void RoundTrip_KeepsBrokenParts()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());
            Car original = factory.Create(CarBrand.Vaz, Season.Spring);

            string text = CarSerializer.Serialize(original);
            Car restored = CarSerializer.Deserialize(text);

            int originalBroken = 0;
            int restoredBroken = 0;

            foreach (Detail detail in original.GetAllDetails())
            {
                if (detail.IsBroken)
                {
                    originalBroken++;
                }
            }

            foreach (Detail detail in restored.GetAllDetails())
            {
                if (detail.IsBroken)
                {
                    restoredBroken++;
                }
            }

            Assert.AreEqual(originalBroken, restoredBroken, "поломки восстановлены");
        }

        [Test]
        public void RoundTrip_KeepsPartBrands()
        {
            var factory = new CarFactory(new SeededRandom(5));
            Car original = factory.Create(CarBrand.Volga, Season.Winter);

            Car restored = CarSerializer.Deserialize(CarSerializer.Serialize(original));

            foreach (Detail detail in original.GetAllDetails())
            {
                Assert.IsTrue(restored.TryGetDetail(detail.Type, out Detail copy), "деталь на месте: " + detail.Name);
                Assert.AreEqual(detail.BrandName, copy.BrandName, "бренд детали сохранён: " + detail.Name);
            }
        }
    }
}
