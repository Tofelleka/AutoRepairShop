namespace AutoRepairShop
{
    internal class ServiceAccess
    {
        private readonly bool _bodyWorkshop;
        private readonly bool _electricWorkshop;
        private readonly bool _bodyBusy;

        public ServiceAccess(bool bodyWorkshop, bool electricWorkshop, bool bodyBusy = false)
        {
            _bodyWorkshop = bodyWorkshop;
            _electricWorkshop = electricWorkshop;
            _bodyBusy = bodyBusy;
        }

        public bool CanRepair(PartCategory category)
        {
            switch (category)
            {
                case PartCategory.Body:
                    return _bodyWorkshop && _bodyBusy == false;

                case PartCategory.Electric:
                    return _electricWorkshop;

                default:
                    return true;
            }
        }

        public string GetLockReason(PartCategory category)
        {
            if (CanRepair(category))
            {
                return string.Empty;
            }

            if (category == PartCategory.Body && _bodyWorkshop && _bodyBusy)
            {
                return "бокс занят";
            }

            return "нужен бокс";
        }
    }
}
