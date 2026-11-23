namespace AutoRepairShop
{
    internal class PartsShop
    {
        private readonly PartsFactory _partsFactory;
        private readonly Warehouse _warehouse;
        private readonly CashRegister _cashRegister;

        internal PartsShop(PartsFactory partsFactory, Warehouse warehouse, CashRegister cashRegister)
        {
            _partsFactory = partsFactory;
            _warehouse = warehouse;
            _cashRegister = cashRegister;
        }

        internal PurchaseReport Buy(DetailType type)
        {
            if (type != DetailType.Exit)
            {
                if (_partsFactory.GetCatalog().TryGetValue(type, out Detail detail))
                {
                    if (_warehouse.TryAdd())
                    {
                        if (_cashRegister.TryPay(detail.Price))
                        {
                            return new PurchaseReport(PurchaseOutcome.Bought, detail);
                        }
                        else
                        {
                            return new PurchaseReport(PurchaseOutcome.NotEnoughMoney, detail);
                        }
                    }
                    else
                    {
                        return new PurchaseReport(PurchaseOutcome.WarehouseIsFull, detail);
                    }
                }
            }

            return null;
        }
    }
}