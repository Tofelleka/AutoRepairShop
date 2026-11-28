using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class RepairWithoutReplacementTests
    {
        [Test]
        public static void Repair_WorksWithoutPart()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.CoolingRadiator);

            car.TryGetDetail(DetailType.CoolingRadiator, out Detail radiator);
            radiator.MarkRepairable();

            DetailRepairReport report = garage.RepairWithoutReplacement(DetailType.CoolingRadiator, car, 1m);

            Assert.AreEqual(RepairOutcome.Repaired, report.Outcome, "деталь отремонтирована без замены");
            Assert.IsTrue(report.Charge > 0m, "за работу платят");
            Assert.AreEqual(0, warehouse.TotalQuantity, "склад не тратится");
            Assert.IsTrue(car.CarIsBroken() == false, "машина исправна");
        }

        [Test]
        public static void Repair_NotRepairableDetail_Refused()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Clutch);

            DetailRepairReport report = garage.RepairWithoutReplacement(DetailType.Clutch, car, 1m);

            Assert.AreEqual(RepairOutcome.NotRepairable, report.Outcome, "сцепление без замены не ремонтируют");
            Assert.IsTrue(car.CarIsBroken(), "деталь осталась сломанной");
        }

        [Test]
        public static void Repair_ChargeIsLowerThanReplacement()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();
            Garage garage = TestData.CreateGarage(warehouse);
            Car car = TestData.CreateCarWithBroken(DetailType.Starter);

            car.TryGetDetail(DetailType.Starter, out Detail starter);
            starter.MarkRepairable();

            DetailRepairReport repair = garage.RepairWithoutReplacement(DetailType.Starter, car, 1m);

            Assert.IsTrue(repair.Charge < TestData.StandardPayout(DetailType.Starter, 1m),
                "ремонт дешевле замены для клиента");
        }

        [Test]
        public static void RepairableTypes_AreMarkedInCatalog()
        {
            Assert.IsTrue(DetailsCatalog.CanRepair(DetailType.Starter), "стартер ремонтируется");
            Assert.IsTrue(DetailsCatalog.CanRepair(DetailType.CoolingRadiator), "радиатор ремонтируется");
            Assert.IsTrue(DetailsCatalog.CanRepair(DetailType.Clutch) == false, "сцепление только менять");
        }
    }
}
