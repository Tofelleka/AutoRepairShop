using System;

namespace AutoRepairShop
{
    internal class PartsShop
    {
        private readonly CashRegister _cashRegister;

        public PartsShop(CashRegister cashRegister)
        {
            if (cashRegister == null)
            {
                throw new ArgumentNullException(nameof(cashRegister));
            }

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

            _cashRegister.Pay(detail.Price);

            return new PurchaseReport(PurchaseOutcome.Bought, detail);
        }
    }
}
