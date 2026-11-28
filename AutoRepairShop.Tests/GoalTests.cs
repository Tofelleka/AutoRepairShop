using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    internal static class GoalTests
    {
        [Test]
        public static void Goal_NotReachedBelowTarget()
        {
            var goal = new GameGoal("Дом", 5_000_000m);

            Assert.IsTrue(goal.IsReached(4_999_999m) == false, "на один рубль меньше — цель не достигнута");
        }

        [Test]
        public static void Goal_ReachedAtTarget()
        {
            var goal = new GameGoal("Дом", 5_000_000m);

            Assert.IsTrue(goal.IsReached(5_000_000m), "ровно цель — победа");
        }

        [Test]
        public static void Goal_ProgressClamped()
        {
            var goal = new GameGoal("Дом", 5_000_000m);

            Assert.AreEqual(0, goal.GetProgress(0m), "ноль денег — ноль процентов");
            Assert.AreEqual(50, goal.GetProgress(2_500_000m), "половина суммы — 50%");
            Assert.AreEqual(100, goal.GetProgress(6_000_000m), "больше цели — 100%");
        }
    }
}
