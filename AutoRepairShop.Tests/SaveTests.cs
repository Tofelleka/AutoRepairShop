using System;
using System.Collections.Generic;
using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class SaveTests
    {
        [Test]
        public void Save_GetPathUsesSlots()
        {
            string first = GameSave.GetPath(1);
            string second = GameSave.GetPath(2);
            string third = GameSave.GetPath(3);

            Assert.IsTrue(first.EndsWith("save1.txt"), "слот 1 — save1.txt");
            Assert.IsTrue(second.EndsWith("save2.txt"), "слот 2 — save2.txt");
            Assert.IsTrue(third.EndsWith("save3.txt"), "слот 3 — save3.txt");
            Assert.IsTrue(first != second && second != third, "слоты не должны совпадать");
        }

        [Test]
        public void Save_InvalidSlotThrows()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => GameSave.GetPath(0),
                "нулевой слот запрещён");

            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => GameSave.GetPath(4),
                "четвёртый слот запрещён");
        }

        [Test]
        public void Save_RoundTrip()
        {
            string path = TestFile.CreatePath("ars-save");

            try
            {
                var levels = new Dictionary<UpgradeType, int>
                {
                    { UpgradeType.Warehouse, 2 },
                    { UpgradeType.Marketing, 1 },
                    { UpgradeType.Skill, 3 }
                };

                var stock = new Dictionary<DetailType, int>
                {
                    { DetailType.Starter, 5 },
                    { DetailType.Clutch, 2 }
                };

                var shift = new ShiftStatistics(7, 20, 15, 5, 100_000m, 5_000m, 105_000m, 40_000m, 30_000m, 4);
                var factory = new CarFactory(new AlwaysHealthyRandom());
                Car queuedCar = factory.Create(CarBrand.Vaz, Season.Spring);
                Car postponedCar = factory.Create(CarBrand.Volga, Season.Autumn);

                queuedCar.AddTuning(TuningType.Power);
                queuedCar.NeedTireChange();

                var save = new GameSave(
                    123_456m,
                    8,
                    levels,
                    stock,
                    shift,
                    5,
                    new[] { CourseType.Izh },
                    777_000m,
                    new[] { new DeliveryOrder(DetailType.Starter, "АвтоВАЗ", 9) },
                    new[] { queuedCar },
                    postponedCar,
                    new System.Collections.Generic.Dictionary<int, int> { { 0, 75 }, { 1, 15 } },
                    new System.Collections.Generic.Dictionary<int, int> { { 0, 2 }, { 1, 3 } },
                    new System.Collections.Generic.Dictionary<int, int> { { 0, -1 }, { 1, 0 } },
                    factory.Create(CarBrand.Izh, Season.Spring));

                TestFile.Run(() => GameSave.Write(path, save));

                Assert.IsTrue(GameSave.Exists(path), "файл сохранения должен появиться");
                Assert.IsTrue(GameSave.Exists(path + ".tmp") == false, "временный файл не должен оставаться");
                Assert.IsTrue(GameSave.TryRead(path, out GameSave loaded), "сохранение должно читаться");

                Assert.AreEqual(123_456m, loaded.Money, "деньги восстановлены");
                Assert.AreEqual(8, loaded.Day, "день восстановлен");
                Assert.AreEqual(2, loaded.Levels[UpgradeType.Warehouse], "уровень склада восстановлен");
                Assert.AreEqual(5, loaded.Stock[DetailType.Starter], "остаток стартера восстановлен");
                Assert.AreEqual(7, loaded.Shift.Days, "дни смены восстановлены");
                Assert.AreEqual(4, loaded.Shift.Referrals, "рекомендации смены восстановлены");
                Assert.AreEqual(5, loaded.BodyBusyUntilDay, "занятость кузовного бокса восстановлена");
                Assert.AreEqual(1, loaded.Courses.Count, "курс восстановлен");
                Assert.AreEqual(777_000m, loaded.Earned, "накопления восстановлены");
                Assert.AreEqual(1, loaded.Deliveries.Count, "доставка восстановлена");
                Assert.AreEqual(1, loaded.QueueCars.Count, "очередь машин восстановлена");
                Assert.IsTrue(loaded.QueueCars[0].HasTuning(TuningType.Power), "тюнинг машины из очереди восстановлен");
                Assert.IsTrue(loaded.QueueCars[0].NeedsTireChange, "шиномонтаж машины из очереди восстановлен");
                Assert.IsTrue(loaded.PostponedCar != null, "отложенная машина восстановлена");
                Assert.AreEqual(CarBrand.Volga, loaded.PostponedCar.Brand, "марка отложенной машины сохранена");
                Assert.AreEqual(75, loaded.OwnerScores[0], "отношение владельца сохранено");
                Assert.AreEqual(15, loaded.OwnerScores[1], "второе отношение сохранено");
                Assert.AreEqual(3, loaded.OwnerFlags[1], "флаги владельца сохранены");
                Assert.IsTrue(loaded.CurrentCar != null, "текущая машина восстановлена");
            }
            finally
            {
                TestFile.Delete(path);
            }
        }

        [Test]
        public void Save_CorruptedFile_ReturnsFalse()
        {
            string path = TestFile.CreatePath("ars-save");

            try
            {
                TestFile.Write(path, "это не сохранение");

                Assert.IsTrue(GameSave.TryRead(path, out GameSave _) == false, "битый файл не читается");
            }
            finally
            {
                TestFile.Delete(path);
            }
        }

        [Test]
        public void Save_MissingFile_ReturnsFalse()
        {
            Assert.IsTrue(GameSave.TryRead(TestFile.CreatePath("ars-save"), out GameSave _) == false,
                "нет файла — нет сохранения");
        }

        [Test]
        public void Save_ExistsChecksFile()
        {
            string path = TestFile.CreatePath("ars-save");

            Assert.IsTrue(GameSave.Exists(path) == false, "файла ещё нет");

            TestFile.Write(path, "version=3");

            try
            {
                Assert.IsTrue(GameSave.Exists(path), "файл появился");
            }
            finally
            {
                TestFile.Delete(path);
            }
        }
    }
}
