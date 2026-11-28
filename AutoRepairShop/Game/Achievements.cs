using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Achievements
    {
        private readonly HashSet<AchievementType> _unlocked;

        public Achievements() : this(new List<AchievementType>())
        {
        }

        public Achievements(IEnumerable<AchievementType> unlocked)
        {
            if (unlocked == null)
            {
                throw new ArgumentNullException(nameof(unlocked));
            }

            _unlocked = new HashSet<AchievementType>(unlocked);
        }

        public IReadOnlyCollection<AchievementType> All => _unlocked;

        public bool IsUnlocked(AchievementType type) => _unlocked.Contains(type);

        public bool Unlock(AchievementType type) => _unlocked.Add(type);

        public static string GetTitle(AchievementType type)
        {
            switch (type)
            {
                case AchievementType.FirstRepair:
                    return "Первый ремонт";

                case AchievementType.FirstBodyRepair:
                    return "Кузовной мастер";

                case AchievementType.FirstElectricRepair:
                    return "Электрик";

                case AchievementType.FirstFluidChange:
                    return "Замена жидкости";

                case AchievementType.FirstUpgrade:
                    return "Первое улучшение";

                case AchievementType.FirstRepaint:
                    return "Полный перекрас";

                case AchievementType.Millionaire:
                    return "Миллион в кассе";

                case AchievementType.Painter:
                    return "Художник";

                case AchievementType.RepaintMaster:
                    return "Мастер перекраса";

                case AchievementType.CourseMaster:
                    return "Мастер на все руки";

                default:
                    return "Всё прокачано";
            }
        }
    }
}
