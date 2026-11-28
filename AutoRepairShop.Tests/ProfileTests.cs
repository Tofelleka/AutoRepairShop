using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class ProfileTests
    {
        [Test]
        public void Unlock_ReturnsTrueOnlyFirstTime()
        {
            var profile = new Profile(0m, 0, new AchievementType[0]);

            Assert.IsTrue(profile.Unlock(AchievementType.FirstRepair), "первое открытие даёт true");
            Assert.IsTrue(profile.Unlock(AchievementType.FirstRepair) == false, "повторное открытие даёт false");
        }

        [Test]
        public void Record_UpdatesOnlyIfBetter()
        {
            var profile = new Profile(1000m, 3, new AchievementType[0]);

            Assert.IsTrue(profile.BeatsRecord(1500m), "1500 лучше рекорда");

            profile.UpdateRecord(1500m, 5);
            Assert.AreEqual(1500m, profile.BestMoney, "рекорд обновился");
            Assert.AreEqual(5, profile.BestDays, "день рекорда обновился");

            profile.UpdateRecord(900m, 9);
            Assert.AreEqual(1500m, profile.BestMoney, "худший результат не перезаписывает рекорд");
        }

        [Test]
        public void Profile_RoundTrip()
        {
            string path = TestFile.CreatePath("ars-profile");

            try
            {
                var profile = new Profile(500m, 7, new[] { AchievementType.FirstRepair });
                profile.Unlock(AchievementType.Millionaire);
                TestFile.Run(() => profile.Save(path));

                Profile loaded = Profile.Load(path);

                Assert.AreEqual(500m, loaded.BestMoney, "рекорд восстановлен");
                Assert.AreEqual(7, loaded.BestDays, "день рекорда восстановлен");
                Assert.AreEqual(2, loaded.Achievements.Count, "достижения восстановлены");
            }
            finally
            {
                TestFile.Delete(path);
            }
        }
    }
}
