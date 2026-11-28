using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class GarageTests
    {
        [Test]
        public void RepairBrokenDetail_PaysPayout()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);

            DetailRepairReport report = garage.RepairDetail(DetailType.Starter, car, 1m);

            Assert.AreEqual(RepairOutcome.ReplacedBroken, report.Outcome, "сломанная деталь должна быть заменена");
            Assert.AreEqual(TestData.StandardPayout(DetailType.Starter, 1m), report.Charge,
                "выплата = цена детали + работа");
            Assert.IsTrue(car.CarIsBroken() == false, "после замены машина должна быть исправна");
        }

        [Test]
        public void RepairBrokenDetail_TakesOneFromWarehouse()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);
            int before = warehouse.GetQuantityDetail(DetailType.Starter);

            garage.RepairDetail(DetailType.Starter, car, 1m);

            Assert.AreEqual(before - 1, warehouse.GetQuantityDetail(DetailType.Starter),
                "деталь должна уйти со склада");
        }

        [Test]
        public void RepairHealthyDetail_NoPayout()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);
            int before = warehouse.GetQuantityDetail(DetailType.Generator);

            DetailRepairReport report = garage.RepairDetail(DetailType.Generator, car, 1m);

            Assert.AreEqual(RepairOutcome.ReplacedHealthy, report.Outcome, "исправная деталь даёт ReplacedHealthy");
            Assert.AreEqual(0m, report.Charge, "за исправную деталь выплаты нет");
            Assert.AreEqual(before - 1, warehouse.GetQuantityDetail(DetailType.Generator),
                "деталь всё равно уходит со склада");
        }

        [Test]
        public void RepairWithoutStock_NoDetailInStock()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);

            DetailRepairReport report = garage.RepairDetail(DetailType.Starter, car, 1m);

            Assert.AreEqual(RepairOutcome.NoDetailInStock, report.Outcome, "без запаса — NoDetailInStock");
            Assert.AreEqual(0m, report.Charge, "выплаты нет");
        }

        [Test]
        public void RepairBodyDetail_WorksLikeEngine()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Door);

            DetailRepairReport report = garage.RepairDetail(DetailType.Door, car, 1m);

            Assert.AreEqual(RepairOutcome.ReplacedBroken, report.Outcome, "кузовная деталь чинится так же");
            Assert.AreEqual(TestData.StandardPayout(DetailType.Door, 1m), report.Charge,
                "выплата по кузовной детали");
        }

        [Test]
        public void RefuseCar_FixedFine()
        {
            Garage garage = TestData.CreateGarage(TestData.CreateWarehouse());

            RepairReport report = garage.RefuseRepairs();

            Assert.AreEqual(2000m, report.Fine, "штраф за отказ до ремонта фиксированный");
        }

        [Test]
        public void RefuseFurther_FineForEachRemaining()
        {
            Garage garage = TestData.CreateGarage(TestData.CreateWarehouse());
            Car car = TestData.CreateCarWithBroken(DetailType.Starter, DetailType.Generator);

            RepairReport report = garage.RefuseFurtherRepairs(5000m, car);

            Assert.AreEqual(5000m, report.Revenue, "выручка сохраняется");
            Assert.AreEqual(600m, report.Fine, "300 за каждую непочиненную деталь");
        }

        [Test]
        public void RepairWithSkillRate_PaysMoreForWork()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);

            DetailRepairReport report = garage.RepairDetail(DetailType.Starter, car, 1.1m);

            Assert.AreEqual(TestData.StandardPayout(DetailType.Starter, 1.1m), report.Charge,
                "коэффициент должен применяться к работе");
        }

        [Test]
        public void CheapFluid_BreaksRelatedDetail()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();
            PartTemplate oil = DetailsCatalog.GetTemplate(DetailType.EngineOil);
            warehouse.TryAdd(new Detail(oil, DetailsCatalog.Brands[0]));

            var garage = new Garage(warehouse, new AlwaysRiskyRandom());
            Car car = TestData.CreateCarWithBroken(DetailType.EngineOil);

            DetailRepairReport report = garage.RepairDetail(DetailType.EngineOil, car, 1m);

            Assert.IsTrue(report.RelatedBroken != null, "дешёвая жидкость должна ломать связанную деталь");
            Assert.IsTrue(car.TryGetDetail(DetailType.Starter, out Detail starter) && starter.IsBroken,
                "сломанным должен оказаться стартер");
        }

        [Test]
        public void GoodFluid_DoesNotBreakRelatedDetail()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.EngineOil);

            DetailRepairReport report = garage.RepairDetail(DetailType.EngineOil, car, 1m);

            Assert.IsTrue(report.RelatedBroken == null, "оригинальная жидкость ничего не ломает");
        }
    }
}
