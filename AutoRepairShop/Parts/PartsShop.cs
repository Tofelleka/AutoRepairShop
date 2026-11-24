using System;

namespace AutoRepairShop
{
    internal class PartsShop
    {
        private readonly PartsFactory _partsFactory;
        private readonly Warehouse _warehouse;
        private readonly CashRegister _cashRegister;

        public PartsShop(PartsFactory partsFactory, Warehouse warehouse, CashRegister cashRegister)
        {
            if (partsFactory == null)
            {
                throw new ArgumentNullException(nameof(partsFactory));
            }

            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            if (cashRegister == null)
            {
                throw new ArgumentNullException(nameof(cashRegister));
            }

            _partsFactory = partsFactory;
            _warehouse = warehouse;
            _cashRegister = cashRegister;
        }

        public PurchaseReport Buy(DetailType type)
        {
            if (_partsFactory.GetAll().ContainsKey(type) == false)
            {
                return new PurchaseReport(PurchaseOutcome.UnknownDetail, null);
            }

            Detail detail = _partsFactory.Create(type);

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
