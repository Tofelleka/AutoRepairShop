using System;

namespace AutoRepairShop
{
    internal class PartsFactory
    {
        public Detail Create(DetailType type, Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            PartTemplate template = DetailsCatalog.GetTemplate(type);

            return new Detail(template, brand);
        }

        public Detail Create(DetailType type) => Create(type, DetailsCatalog.StandardBrand);
    }
}
