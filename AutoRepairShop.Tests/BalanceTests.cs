using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class BalanceTests
    {
        [Test]
        public void Simulator_ReachesGoal()
        {
            var simulator = new BalanceSimulator(2026);

            int days = simulator.Run(600);

            Assert.IsTrue(days > 0, "цель должна быть достижима");
        }

        [Test]
        public void Simulator_GoalTakesAboutTwoHundredDays()
        {
            var simulator = new BalanceSimulator(2026);

            int days = simulator.Run(600);

            Assert.IsTrue(days >= 120, "слишком быстро: " + days + " дней");
            Assert.IsTrue(days <= 360, "слишком долго: " + days + " дней");
        }

        [Test]
        public void Simulator_DoesNotLoseEverything()
        {
            var simulator = new BalanceSimulator(2026);

            simulator.Run(600);

            Assert.IsTrue(simulator.Money > 0m, "бот должен оставаться в плюсе");
        }

        [Test]
        public void Simulator_UnlocksIzhCourse()
        {
            var simulator = new BalanceSimulator(2026);

            simulator.Run(600);

            Assert.IsTrue(simulator.HasCourse(CourseType.Izh), "бот должен пройти курсы Иж");
        }

        [Test]
        public void Simulator_UnlocksVolgaCourse()
        {
            var simulator = new BalanceSimulator(2026);

            simulator.Run(600);

            Assert.IsTrue(simulator.HasCourse(CourseType.Volga), "бот должен пройти курсы Волга");
        }

        [Test]
        public void CasualBot_StillReachesGoalInTime()
        {
            var simulator = new BalanceSimulator(2026, 0.3d);

            int days = simulator.Run(900);

            Assert.IsTrue(simulator.IsBankrupt == false, "небрежный игрок не должен разоряться");
            Assert.IsTrue(days > 0, "даже небрежный игрок должен дойти до цели");
            Assert.IsTrue(days <= 400, "слишком долго для казуального игрока: " + days);
        }
    }
}
