using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class PartsShop
    {
        private readonly PartsFactory _partsFactory;
        private readonly Warehouse _warehouse;
        private readonly CashRegister _cashRegister;

        public PartsShop(PartsFactory partsFactory, Warehouse warehouse, CashRegister cashRegister)
        {
            _partsFactory = partsFactory;
            _warehouse = warehouse;
            _cashRegister = cashRegister;
        }

        public PurchaseReport Buy(DetailType type)
        {
            if (_partsFactory.GetCatalog().TryGetValue(type, out Detail detail) == false)
            {
                return new PurchaseReport(PurchaseOutcome.UnknownDetail, null);
            }

            if (_cashRegister.CanAfford(detail.Price) == false)
            {
                return new PurchaseReport(PurchaseOutcome.NotEnoughMoney, detail);
            }

            if (_warehouse.TryAdd(detail) == false)
            {
                return new PurchaseReport(PurchaseOutcome.WarehouseIsFull, detail);
            }

            _cashRegister.Pay(detail.Price);

            return new PurchaseReport(PurchaseOutcome.Bought, detail);
        }
    }
}