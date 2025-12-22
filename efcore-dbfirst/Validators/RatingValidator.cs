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
    public class RatingValidator : ValidationRule
    {
        private static readonly Regex RatingRegex =
            new(@"^(?:[0-4]\.[0-9]|5\.0)$");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Рейтинг обязателен");

            if (input.Contains(','))
                return new ValidationResult(false, "Используйте точку, а не запятую");

            if (!RatingRegex.IsMatch(input))
                return new ValidationResult(false, "Формат: от 0.1 до 5.0 (например 4.5)");

            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var rating))
                return new ValidationResult(false, "Некорректное число");

            if (rating < 0.1m || rating > 5.0m)
                return new ValidationResult(false, "Рейтинг от 0.1 до 5.0");

            return ValidationResult.ValidResult;
        }
    }
}
