using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace efcore_dbfirst.Validators
{
    public class PriceValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Введите цену");

            if (input.Contains(','))
                return new ValidationResult(false, "Используйте точку, а не запятую");

            if (!Regex.IsMatch(input, @"^\d+(\.\d{1,4})?$"))
                return new ValidationResult(false, "Некорректный формат цены");

            if (!decimal.TryParse(
                    input,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var price))
                return new ValidationResult(false, "Некорректное число");

            if (price < 0)
                return new ValidationResult(false, "Цена не может быть отрицательной");

            return ValidationResult.ValidResult;
        }
    }
}
