using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Education
    {
        private readonly HashSet<CourseType> _passed;

        public Education() : this(new List<CourseType>())
        {
        }

        public Education(IEnumerable<CourseType> passed)
        {
            if (passed == null)
            {
                throw new ArgumentNullException(nameof(passed));
            }

            _passed = new HashSet<CourseType>(passed);
        }

        public IReadOnlyCollection<CourseType> Passed => _passed;

        public bool IsPassed(CourseType type) => _passed.Contains(type);

        public bool Pass(CourseType type) => _passed.Add(type);

        public bool HasBrand(CarBrand brand)
        {
            foreach (CourseInfo info in CourseCatalog.All)
            {
                if (info.Brand == brand && IsPassed(info.Type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
