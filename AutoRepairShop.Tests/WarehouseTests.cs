using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class WarehouseTests
    {
        [Test]
        public static void TryTake_EmptyWarehouse_ReturnsFalse()
        {
            Warehouse warehouse = TestData.CreateEmptyWarehouse();

            bool result = warehouse.TryTake(DetailType.Starter, out Detail detail);

            Assert.IsTrue(result == false, "со пустого склада взять нельзя");
            Assert.IsTrue(detail == null, "деталь не должна вернуться");
        }

        [Test]
        public static void TryAdd_OverCapacity_ReturnsFalse()
        {
            var factory = new PartsFactory();
            Warehouse warehouse = TestData.CreateWarehouse();
            Detail extra = factory.Create(DetailType.Generator);

            while (warehouse.TotalQuantity < warehouse.Capacity)
            {
                warehouse.TryAdd(factory.Create(DetailType.Generator));
            }

            bool result = warehouse.TryAdd(extra);

            Assert.IsTrue(result == false, "на полный склад добавить нельзя");
            Assert.AreEqual(warehouse.Capacity, warehouse.TotalQuantity, "запас не превысил вместимость");
        }

        [Test]
        public static void IncreaseCapacity_Increases()
        {
            Warehouse warehouse = TestData.CreateWarehouse();
            int before = warehouse.Capacity;

            warehouse.IncreaseCapacity(15);

            Assert.AreEqual(before + 15, warehouse.Capacity, "вместимость растёт");
        }

        [Test]
        public static void GetQuantity_UnknownType_Throws()
        {
            Warehouse warehouse = TestData.CreateWarehouse();

            Assert.Throws<System.ArgumentException>(
                () => warehouse.GetQuantityDetail((DetailType)99),
                "неизвестный тип должен приводить к исключению");
        }
    }
}
