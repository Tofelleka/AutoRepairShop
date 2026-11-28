using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class CarTests
    {
        [Test]
        public void Car_AlwaysHasBrokenDetail()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Vaz, Season.Spring);

            Assert.IsTrue(car.CarIsBroken(), "у машины должна быть хотя бы одна поломка");
        }

        [Test]
        public void Car_DetailsFitModel()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            for (int i = 0; i < 20; i++)
            {
                Car car = factory.Create(CarBrand.Vaz, Season.Spring);

                foreach (Detail detail in car.GetAllDetails())
                {
                    PartTemplate template = DetailsCatalog.GetTemplate(detail.Type);

                    Assert.IsTrue(template.FitsModel(car.Name),
                        "деталь не подходит модели: " + detail.Name + " / " + car.Name);
                }
            }
        }

        [Test]
        public void Car_HasAllModelDetails()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Vaz, Season.Spring);

            int expected = 0;

            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(car.Brand, car.Name))
            {
                if (template.Category != PartCategory.Material && template.Category != PartCategory.Tuning)
                {
                    expected++;
                }
            }

            Assert.AreEqual(expected, car.GetAllDetails().Count,
                "машина должна содержать все детали, подходящие её модели");
        }

        [Test]
        public void Car_DetailsHaveBrands()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Vaz, Season.Spring);

            foreach (Detail detail in car.GetAllDetails())
            {
                Assert.IsTrue(string.IsNullOrEmpty(detail.BrandName) == false,
                    "у каждой детали должен быть производитель");
            }
        }

        [Test]
        public void Izh_HasUniqueParts()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Izh, Season.Spring);

            Assert.IsTrue(car.TryGetDetail(DetailType.IzhCarburetor, out Detail _), "у Ижа есть карбюратор");
            Assert.IsTrue(car.TryGetDetail(DetailType.IzhRearAxle, out Detail _), "у Ижа есть задний мост");
            Assert.IsTrue(car.TryGetDetail(DetailType.VolgaWaterPump, out Detail _) == false,
                "волговских деталей у Ижа нет");
        }

        [Test]
        public void Volga_HasUniqueParts()
        {
            var factory = new CarFactory(new AlwaysHealthyRandom());

            Car car = factory.Create(CarBrand.Volga, Season.Spring);

            Assert.IsTrue(car.TryGetDetail(DetailType.VolgaPowerSteering, out Detail _), "у Волги есть ГУР");
            Assert.IsTrue(car.TryGetDetail(DetailType.VolgaRearSprings, out Detail _), "у Волги есть рессоры");
            Assert.IsTrue(car.TryGetDetail(DetailType.IzhCarburetor, out Detail _) == false,
                "ижевских деталей у Волги нет");
        }

        [Test]
        public void ModelSpecificParts_FitOnlyTheirModels()
        {
            Assert.IsTrue(HasTemplate(CarBrand.Vaz, "ВАЗ - 2108", DetailType.Vaz2108Clutch),
                "сцепление Самары подходит 2108");
            Assert.IsTrue(HasTemplate(CarBrand.Vaz, "ВАЗ - 2101", DetailType.Vaz2108Clutch) == false,
                "сцепление Самары не подходит классике");
            Assert.IsTrue(HasTemplate(CarBrand.Vaz, "ВАЗ - 2101", DetailType.VazClassicIgnition),
                "трамблёр подходит классике");
            Assert.IsTrue(HasTemplate(CarBrand.Izh, "Иж - 2715", DetailType.Izh2715BodyPanel),
                "борт фургона подходит Иж-2715");
            Assert.IsTrue(HasTemplate(CarBrand.Izh, "Иж - 2126", DetailType.Izh2715BodyPanel) == false,
                "борт фургона не подходит хэтчбеку");
        }

        [Test]
        public void CarsQueue_UsesOnlyAllowedBrands()
        {
            var random = new SeededRandom(42);
            var queue = new CarsQueue(new CarFactory(random), random);

            queue.AddCars(4, new[] { CarBrand.Vaz }, Season.Spring);

            while (queue.HasCars)
            {
                Assert.AreEqual(CarBrand.Vaz, queue.DequeueCar().Brand, "в очереди только разрешённые марки");
            }
        }

        private static bool HasTemplate(CarBrand brand, string model, DetailType type)
        {
            foreach (PartTemplate template in DetailsCatalog.GetTemplatesFor(brand, model))
            {
                if (template.Type == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
