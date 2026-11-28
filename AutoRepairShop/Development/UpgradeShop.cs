using System;

namespace AutoRepairShop
{
    internal class UpgradeShop
    {
        private readonly CashRegister _cashRegister;
        private readonly ServiceDevelopment _development;

        public UpgradeShop(CashRegister cashRegister, ServiceDevelopment development)
        {
            if (cashRegister == null)
            {
                throw new ArgumentNullException(nameof(cashRegister));
            }

            if (development == null)
            {
                throw new ArgumentNullException(nameof(development));
            }

            _cashRegister = cashRegister;
            _development = development;
        }

        public UpgradeReport Buy(UpgradeType type)
        {
            UpgradeInfo info = UpgradeCatalog.Get(type);
            int level = _development.GetLevel(type);

            if (info.HasNextLevel(level) == false)
            {
                return new UpgradeReport(UpgradeOutcome.MaxLevelReached, type, info.Name, 0m, level);
            }

            decimal price = info.GetPrice(level);

            if (_cashRegister.CanAfford(price) == false)
            {
                return new UpgradeReport(UpgradeOutcome.NotEnoughMoney, type, info.Name, price, level);
            }

            _cashRegister.Pay(price);
            _development.ApplyUpgrade(type);

            return new UpgradeReport(UpgradeOutcome.Bought, type, info.Name, price, level + 1);
        }
    }
}
