using AutoRepairShop;

namespace AutoRepairShop.Tests
{
    public class EducationTests
    {
        [Test]
        public void CourseCatalog_HasTwoCourses()
        {
            Assert.AreEqual(2, CourseCatalog.All.Count, "два курса: Иж и Волга");
        }

        [Test]
        public void CourseThresholds()
        {
            Assert.AreEqual(500_000m, CourseCatalog.Get(CourseType.Izh).RequiredEarned,
                "курсы Иж открываются после 500 000");

            Assert.AreEqual(1_000_000m, CourseCatalog.Get(CourseType.Volga).RequiredEarned,
                "курсы Волга открываются после 1 000 000");
        }

        [Test]
        public void Education_PassIsIdempotent()
        {
            var education = new Education();

            Assert.IsTrue(education.IsPassed(CourseType.Izh) == false, "сначала курс не пройден");
            Assert.IsTrue(education.Pass(CourseType.Izh), "первое прохождение даёт true");
            Assert.IsTrue(education.Pass(CourseType.Izh) == false, "повторное — false");
            Assert.IsTrue(education.IsPassed(CourseType.Izh), "курс отмечен пройденным");
        }

        [Test]
        public void Education_UnlocksBrand()
        {
            var education = new Education();

            Assert.IsTrue(education.HasBrand(CarBrand.Izh) == false, "Иж закрыт без курса");

            education.Pass(CourseType.Izh);

            Assert.IsTrue(education.HasBrand(CarBrand.Izh), "Иж открыт после курса");
            Assert.IsTrue(education.HasBrand(CarBrand.Volga) == false, "Волга всё ещё закрыта");
        }

        [Test]
        public void CourseCatalog_MapsBrands()
        {
            Assert.AreEqual(CarBrand.Izh, CourseCatalog.Get(CourseType.Izh).Brand, "курс Иж открывает Иж");
            Assert.AreEqual(CarBrand.Volga, CourseCatalog.Get(CourseType.Volga).Brand, "курс Волга открывает Волгу");
        }
    }
}
