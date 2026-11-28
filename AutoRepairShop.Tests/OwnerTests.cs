using System.Collections.Generic;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class OwnerTests
    {
        private static Owner CreateOwner(int relationship) =>
            new Owner(
                0,
                "Тест",
                CarBrand.Vaz,
                "ВАЗ - 2108",
                "история",
                new[] { "факт" },
                relationship,
                Nationality.Russian);

        [Test]
        public void BaseHasTwoHundredOwners()
        {
            Assert.AreEqual(200, OwnerBase.CreateAll().Count, "в базе 200 владельцев");
        }

        [Test]
        public void OwnersHaveUniqueNames()
        {
            var names = new HashSet<string>();

            foreach (Owner owner in OwnerBase.CreateAll())
            {
                Assert.IsTrue(names.Add(owner.Name), "имя уникально: " + owner.Name);
            }
        }

        [Test]
        public void StoriesAreUnique()
        {
            var stories = new HashSet<string>();

            foreach (Owner owner in OwnerBase.CreateAll())
            {
                Assert.IsTrue(stories.Add(owner.Story), "история уникальна: " + owner.Name);
            }

            Assert.AreEqual(OwnerBase.OwnerCount, stories.Count, "у каждого своя история");
        }

        [Test]
        public void FactPoolsHaveExpectedEntries()
        {
            foreach (CarBrand brand in new[] { CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga })
            {
                List<string> pool = OwnerBase.GetFactPool(brand);
                var unique = new HashSet<string>(pool);

                Assert.AreEqual(OwnerBase.FactsPerBrand, pool.Count, "в пуле 100 фактов: " + brand);
                Assert.AreEqual(pool.Count, unique.Count, "факты в пуле не повторяются: " + brand);
            }
        }

        [Test]
        public void OwnersHaveTwoDifferentFacts()
        {
            foreach (Owner owner in OwnerBase.CreateAll())
            {
                Assert.IsTrue(owner.Facts.Count >= OwnerBase.FactsPerOwner, "минимум два факта: " + owner.Name);
                Assert.IsTrue(owner.Facts.Count <= OwnerBase.FactsPerOwner + 1, "не больше трёх фактов: " + owner.Name);
                Assert.IsTrue(owner.Facts[0] != owner.Facts[1], "факты разные: " + owner.Name);
            }
        }

        [Test]
        public void TenNationalitiesPresent()
        {
            var nationalities = new HashSet<Nationality>();

            foreach (Owner owner in OwnerBase.CreateAll())
            {
                nationalities.Add(owner.Nationality);
                Assert.IsTrue(string.IsNullOrEmpty(owner.GetGreeting()) == false, "есть приветствие");
                Assert.IsTrue(string.IsNullOrEmpty(owner.GetPraise()) == false, "есть похвала");
                Assert.IsTrue(string.IsNullOrEmpty(owner.GetComplaint()) == false, "есть жалоба");
            }

            Assert.AreEqual(10, nationalities.Count, "десять национальностей");
        }

        [Test]
        public void HundredOwnersAcquaintedAtStart()
        {
            int acquainted = 0;

            foreach (Owner owner in OwnerBase.CreateAll())
            {
                if (owner.IsAcquainted)
                {
                    acquainted++;
                }
            }

            Assert.AreEqual(OwnerBase.AcquaintedAtStart, acquainted, "сначала знакомы сто человек");
        }

        [Test]
        public void ReferralFlagsWork()
        {
            Owner owner = CreateOwner(90);

            Assert.IsFalse(owner.HasReferred, "сначала не приводил никого");

            owner.MarkReferred();
            owner.MakeAcquainted();

            Assert.IsTrue(owner.HasReferred, "флаг рекомендации поставлен");
            Assert.IsTrue(owner.IsAcquainted, "знакомство отмечено");

            owner.RestoreFlags(false, false, -1);

            Assert.IsFalse(owner.HasReferred, "флаг сброшен");
            Assert.IsFalse(owner.IsAcquainted, "знакомство сброшено");
        }

        [Test]
        public void RelationshipChangesAndClamps()
        {
            Owner owner = CreateOwner(50);

            owner.ChangeRelationship(60);
            Assert.AreEqual(100, owner.Relationship, "больше 100 не бывает");

            owner.ChangeRelationship(-500);
            Assert.AreEqual(0, owner.Relationship, "меньше 0 не бывает");
        }

        [Test]
        public void AngryOwnerIsDetected()
        {
            Owner owner = CreateOwner(50);

            owner.ChangeRelationship(-35);
            Assert.IsTrue(owner.IsAngry, "ниже 20 — ярость");
        }

        [Test]
        public void StatusesAreNamed()
        {
            Assert.AreEqual("нейтрально", CreateOwner(50).GetStatus(), "50 — нейтрально");
            Assert.AreEqual("друг", CreateOwner(85).GetStatus(), "85 — друг");
            Assert.AreEqual("приятель", CreateOwner(65).GetStatus(), "65 — приятель");
            Assert.AreEqual("недоволен", CreateOwner(30).GetStatus(), "30 — недоволен");
            Assert.AreEqual("в ярости", CreateOwner(15).GetStatus(), "15 — ярость");
        }

        [Test]
        public void PayMultiplierDependsOnRelationship()
        {
            Assert.AreEqual(1.10m, CreateOwner(90).GetPayMultiplier(), "друг платит с надбавкой");
            Assert.AreEqual(1.05m, CreateOwner(65).GetPayMultiplier(), "приятель чуть больше");
            Assert.AreEqual(1.00m, CreateOwner(50).GetPayMultiplier(), "нейтральный — по прайсу");
            Assert.AreEqual(0.90m, CreateOwner(30).GetPayMultiplier(), "недовольный торгуется");
        }

        [Test]
        public void CarKeepsOwnerId()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());
            Car car = factory.Create(CarBrand.Vaz, Season.Spring);

            car.AssignOwner(42);

            Car restored = CarSerializer.Deserialize(CarSerializer.Serialize(car));

            Assert.AreEqual(42, restored.OwnerId, "номер владельца сохраняется");
        }
    }
}
