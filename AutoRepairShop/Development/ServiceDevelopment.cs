namespace AutoRepairShop
{
    internal class ServiceDevelopment
    {
        public int WarehouseLevel { get; private set; }

        public bool CanUpgradeWarehouse => WarehouseUpgrade.HasNextLevel(WarehouseLevel);

        public decimal WarehouseUpgradePrice => CanUpgradeWarehouse ? WarehouseUpgrade.GetPrice(WarehouseLevel) : 0m;

        public void ApplyWarehouseUpgrade() => WarehouseLevel++;
    }
}