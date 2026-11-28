using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class LegendTests
    {
        [Test]
        public void LegendaryFactsAreRare()
        {
            int legendary = 0;

            foreach (Owner owner in OwnerBase.CreateAll())
            {
                foreach (string fact in owner.Facts)
                {
                    if (fact.Contains("Легенда:"))
                    {
                        legendary++;
                    }
                }
            }

            Assert.AreEqual(2, legendary, "легенда есть у одного владельца из сотни");
        }

        [Test]
        public void LegendaryOwnerHasThreeFacts()
        {
            var owners = OwnerBase.CreateAll();

            Assert.AreEqual(3, owners[0].Facts.Count, "у нулевого владельца легенда");
            Assert.AreEqual(2, owners[1].Facts.Count, "у обычного владельца два факта");
        }

        [Test]
        public void FarewellDependsOnRelationship()
        {
            var friendly = new Owner(0, "A", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 90, Nationality.Russian);
            var cold = new Owner(1, "B", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 30, Nationality.Russian);
            var angry = new Owner(2, "C", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 10, Nationality.Russian);

            Assert.IsTrue(friendly.GetFarewell() != cold.GetFarewell(), "прощание зависит от отношения");
            Assert.AreEqual("Больше не приеду.", angry.GetFarewell(), "разъярённый прощается навсегда");
        }
    }
}
