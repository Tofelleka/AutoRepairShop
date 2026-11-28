using System;

namespace AutoRepairShop
{
    internal class PartsShop
    {
        private readonly Warehouse _warehouse;
        private readonly CashRegister _cashRegister;

        public PartsShop(Warehouse warehouse, CashRegister cashRegister)
        {
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            if (cashRegister == null)
            {
                throw new ArgumentNullException(nameof(cashRegister));
            }

            _warehouse = warehouse;
            _cashRegister = cashRegister;
        }

        public PurchaseReport Buy(DetailType type, Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            if (DetailsCatalog.TryGetTemplate(type, out PartTemplate template) == false)
            {
                return new PurchaseReport(PurchaseOutcome.UnknownDetail, null);
            }

            var detail = new Detail(template, brand);

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
