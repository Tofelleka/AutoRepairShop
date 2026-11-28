using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class ContentTests
    {
        [Test]
        public void EveryModelHasTwoFacts()
        {
            foreach (CarBrand brand in new[] { CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga })
            {
                foreach (string model in CarNames.GetNames(brand))
                {
                    Assert.AreEqual(2, ModelFacts.GetFacts(model).Count, "два факта о модели: " + model);
                }
            }
        }

        [Test]
        public void ModelFactsCoverAllModels()
        {
            int models = 0;

            foreach (CarBrand brand in new[] { CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga })
            {
                models += CarNames.GetNames(brand).Count;
            }

            Assert.AreEqual(16, models, "шестнадцать моделей");
        }

        [Test]
        public void UkrainianDistortsText()
        {
            string original = "это очень хорошее дело, что машина едет";
            string distorted = Accents.Distort(original, Nationality.Ukrainian);

            Assert.IsTrue(distorted != original, "текст меняется под акцент");
            Assert.IsTrue(distorted.Contains("шо"), "украинец говорит «шо»");
            Assert.IsTrue(distorted.Contains("дуже"), "украинец говорит «дуже»");
        }

        [Test]
        public void GermanDistortsText()
        {
            string original = "хорошо, что эта машина едет";
            string distorted = Accents.Distort(original, Nationality.German);

            Assert.IsTrue(distorted.Contains("sehr gut"), "немец вставляет немецкое");
            Assert.IsTrue(distorted.Contains("Auto"), "машина превращается в Auto");
        }

        [Test]
        public void ModelAttitudeDependsOnNationality()
        {
            string georgian = Accents.GetModelAttitude(Nationality.Georgian, "ВАЗ - 2107");
            string german = Accents.GetModelAttitude(Nationality.German, "ВАЗ - 2107");

            Assert.IsTrue(georgian != german, "отношение к модели зависит от национальности");
            Assert.IsTrue(georgian.Contains("ВАЗ - 2107"), "модель упомянута");
        }

        [Test]
        public void ArmenianSwapsWords()
        {
            string distorted = Accents.Distort("машина едет по дороге", Nationality.Armenian);

            Assert.IsTrue(distorted.StartsWith("едет машина"), "армянин переставляет слова");
        }

        [Test]
        public void TatarMakesEndingMistakes()
        {
            string distorted = Accents.Distort("машина моется и его хвалит", Nationality.Tatar);

            Assert.IsTrue(distorted.Contains("моетца"), "татарин путает окончания");
            Assert.IsTrue(distorted.Contains("ево"), "и говорит «ево»");
        }

        [Test]
        public void GreetingDependsOnRelationship()
        {
            var friend = new Owner(0, "A", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 90, Nationality.Russian);
            var neutral = new Owner(1, "B", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 50, Nationality.Russian);
            var cold = new Owner(2, "C", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 30, Nationality.Russian);
            var angry = new Owner(3, "D", CarBrand.Vaz, "ВАЗ - 2108", "s", new[] { "f" }, 10, Nationality.Russian);

            Assert.IsTrue(friend.GetGreeting() != neutral.GetGreeting(), "друг здоровается иначе");
            Assert.IsTrue(cold.GetGreeting() != neutral.GetGreeting(), "недовольный здоровается холодно");
            Assert.AreEqual("Больше не приеду.", angry.GetGreeting(), "в ярости он уже не здоровается");
        }

        [Test]
        public void OwnerSecondFactMentionsModel()
        {
            foreach (Owner owner in OwnerBase.CreateAll())
            {
                bool mentionsModel = false;

                foreach (string fact in owner.Facts)
                {
                    if (fact.Contains(owner.Model))
                    {
                        mentionsModel = true;
                    }
                }

                Assert.IsTrue(mentionsModel, "один факт — про модель: " + owner.Name);
            }
        }
    }
}
