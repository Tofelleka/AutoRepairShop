using System;

namespace AutoRepairShop
{
    internal class UpgradeShop
    {
        private readonly CashRegister _cashRegister;
        private readonly Warehouse _warehouse;
        private readonly ServiceDevelopment _development;

        public UpgradeShop(CashRegister cashRegister, Warehouse warehouse, ServiceDevelopment development)
        {
            if (cashRegister == null)
            {
                throw new ArgumentNullException(nameof(cashRegister));
            }

            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            if (development == null)
            {
                throw new ArgumentNullException(nameof(development));
            }

            _cashRegister = cashRegister;
            _warehouse = warehouse;
            _development = development;
        }

        public UpgradeReport BuyWarehouseUpgrade()
        {
            if (_development.CanUpgradeWarehouse == false)
            {
                return new UpgradeReport(UpgradeOutcome.MaxLevelReached, 0m, _development.WarehouseLevel);
            }

            decimal price = _development.WarehouseUpgradePrice;

            if (_cashRegister.CanAfford(price) == false)
            {
                return new UpgradeReport(UpgradeOutcome.NotEnoughMoney, price, _development.WarehouseLevel);
            }

            _cashRegister.Pay(price);
            _development.ApplyWarehouseUpgrade();
            _warehouse.IncreaseCapacity(WarehouseUpgrade.CapacityPerLevel);

            return new UpgradeReport(UpgradeOutcome.Bought, price, _development.WarehouseLevel);
        }
    }
}