using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class SeasonTests
    {
        [Test]
        public void HeaterExistsAndTiresAreClientOwned()
        {
            Assert.IsTrue(DetailsCatalog.TryGetTemplate(DetailType.Heater, out PartTemplate _), "печка есть в каталоге");
            Assert.IsTrue(DetailsCatalog.TryGetTemplate(DetailType.Tires, out PartTemplate _) == false,
                "шин на складе нет — они клиентские");
        }

        [Test]
        public void HeaterIsRepairable()
        {
            Assert.IsTrue(DetailsCatalog.CanRepair(DetailType.Heater), "печку можно отремонтировать");
        }

        [Test]
        public void CarsHaveHeater()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Vaz, Season.Winter);

            Assert.IsTrue(car.TryGetDetail(DetailType.Heater, out Detail _), "у машины есть печка");
            Assert.IsTrue(car.TryGetDetail(DetailType.Tires, out Detail _) == false, "шин у машины в деталях нет");
        }

        [Test]
        public void CarsDifferByBrandBodyPanels()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car vaz = factory.Create(CarBrand.Vaz, Season.Spring);
            Car volga = factory.Create(CarBrand.Volga, Season.Spring);

            Assert.IsTrue(vaz.TryGetDetail(DetailType.Door, out Detail _), "у ВАЗа своя дверь");
            Assert.IsTrue(volga.TryGetDetail(DetailType.VolgaDoor, out Detail _), "у Волги своя дверь");
            Assert.IsTrue(volga.TryGetDetail(DetailType.Door, out Detail _) == false, "вазовская дверь Волге не подходит");
        }

        [Test]
        public void FourSeasonsCycle()
        {
            Assert.AreEqual(4, System.Enum.GetValues(typeof(Season)).Length, "четыре сезона");
        }
    }
}
