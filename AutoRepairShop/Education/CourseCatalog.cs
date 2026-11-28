using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class CourseCatalog
    {
        private static readonly List<CourseInfo> Courses = new List<CourseInfo>
        {
            new CourseInfo(CourseType.Izh, "Курсы: Иж", CarBrand.Izh, 500_000m, 80_000m),
            new CourseInfo(CourseType.Volga, "Курсы: Волга", CarBrand.Volga, 1_000_000m, 150_000m)
        };

        public static IReadOnlyList<CourseInfo> All => Courses;

        public static CourseInfo Get(CourseType type)
        {
            foreach (CourseInfo info in Courses)
            {
                if (info.Type == type)
                {
                    return info;
                }
            }

            throw new ArgumentException($"Неизвестный курс: {type}.", nameof(type));
        }
    }
}
