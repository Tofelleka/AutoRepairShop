using System;
using System.Collections.Generic;
using System.Text;

namespace AutoRepairShop
{
    internal static class CarSerializer
    {
        private const char FieldSeparator = '~';
        private const char PartSeparator = ';';
        private const char PartFieldSeparator = ',';
        private const char ListSeparator = ',';

        public static string Serialize(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            var builder = new StringBuilder();

            builder.Append(car.Brand);
            builder.Append(FieldSeparator);
            builder.Append(car.Name);
            builder.Append(FieldSeparator);
            builder.Append(car.NeedsTireChange ? "1" : "0");
            builder.Append(FieldSeparator);
            builder.Append(JoinTunings(car));
            builder.Append(FieldSeparator);
            builder.Append(car.OwnerId);
            builder.Append(FieldSeparator);
            builder.Append(JoinParts(car));

            return builder.ToString();
        }

        public static Car Deserialize(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new FormatException("Пустая запись машины.");
            }

            string[] fields = text.Split(FieldSeparator);

            if (fields.Length != 5 && fields.Length != 6)
            {
                throw new FormatException("Некорректная запись машины.");
            }

            if (Enum.TryParse(fields[0], out CarBrand brand) == false)
            {
                throw new FormatException("Неизвестная марка машины: " + fields[0]);
            }

            int partsField = fields.Length - 1;
            Car car = new Car(brand, fields[1], ParseParts(fields[partsField]));

            car.RestoreTireChange(fields[2] == "1");

            if (fields.Length == 6 && int.TryParse(fields[4], out int ownerId))
            {
                car.AssignOwner(ownerId);
            }

            foreach (string item in fields[3].Split(new[] { ListSeparator }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse(item, out TuningType tuning))
                {
                    car.AddTuning(tuning);
                }
            }

            return car;
        }

        private static string JoinTunings(Car car)
        {
            var parts = new List<string>();

            foreach (TuningType tuning in car.Tunings)
            {
                parts.Add(tuning.ToString());
            }

            return string.Join(ListSeparator.ToString(), parts);
        }

        private static string JoinParts(Car car)
        {
            var parts = new List<string>();

            foreach (Detail detail in car.GetAllDetails())
            {
                parts.Add(string.Join(
                    PartFieldSeparator.ToString(),
                    detail.Type.ToString(),
                    detail.BrandName,
                    detail.IsBroken ? "1" : "0",
                    detail.IsRepairable ? "1" : "0"));
            }

            return string.Join(PartSeparator.ToString(), parts);
        }

        private static Dictionary<DetailType, Detail> ParseParts(string text)
        {
            var details = new Dictionary<DetailType, Detail>();

            foreach (string item in text.Split(new[] { PartSeparator }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] fields = item.Split(PartFieldSeparator);

                if (fields.Length != 4)
                {
                    continue;
                }

                if (Enum.TryParse(fields[0], out DetailType type) == false)
                {
                    continue;
                }

                if (DetailsCatalog.TryGetTemplate(type, out PartTemplate template) == false)
                {
                    continue;
                }

                var detail = new Detail(template, FindBrand(fields[1]));

                detail.RestoreState(fields[2] == "1", fields[3] == "1");

                details[type] = detail;
            }

            if (details.Count == 0)
            {
                throw new FormatException("У машины нет деталей.");
            }

            return details;
        }

        private static Brand FindBrand(string name)
        {
            foreach (Brand brand in DetailsCatalog.Brands)
            {
                if (brand.Name == name)
                {
                    return brand;
                }
            }

            return DetailsCatalog.StandardBrand;
        }
    }
}
