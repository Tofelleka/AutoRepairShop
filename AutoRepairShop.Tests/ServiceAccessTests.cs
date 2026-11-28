using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class ServiceAccessTests
    {
        [Test]
        public void EngineAndFluid_AlwaysAllowed()
        {
            var access = new ServiceAccess(false, false);

            Assert.IsTrue(access.CanRepair(PartCategory.Engine), "двигатель доступен всегда");
            Assert.IsTrue(access.CanRepair(PartCategory.Fluid), "жидкости доступны всегда");
        }

        [Test]
        public void Body_RequiresBox()
        {
            var locked = new ServiceAccess(false, false);
            var unlocked = new ServiceAccess(true, false);

            Assert.IsTrue(locked.CanRepair(PartCategory.Body) == false, "без бокса кузов закрыт");
            Assert.IsTrue(unlocked.CanRepair(PartCategory.Body), "с боксом кузов открыт");
        }

        [Test]
        public void Electric_RequiresBox()
        {
            var locked = new ServiceAccess(false, false);
            var unlocked = new ServiceAccess(false, true);

            Assert.IsTrue(locked.CanRepair(PartCategory.Electric) == false, "без бокса электрика закрыта");
            Assert.IsTrue(unlocked.CanRepair(PartCategory.Electric), "с боксом электрика открыта");
        }
    }
}
