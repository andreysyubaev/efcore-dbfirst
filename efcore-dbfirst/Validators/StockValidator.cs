using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace efcore_dbfirst.Validators
{
    public class StockValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (input == string.Empty)
                return new ValidationResult(false, "Ввод поля обязателен");

            foreach (char c in input)
            {
                if (!char.IsDigit(c) && c != '.' && c != ',')
                    return new ValidationResult(false, "Введите число!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
