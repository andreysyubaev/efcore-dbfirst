using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace efcore_dbfirst.Validators
{
    public class ComboBoxValidator : ValidationRule
    {
        public string ErrorMessage { get; set; } = "Поле обязательно для заполнения";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (value is int intValue && intValue == 0))
            {
                return new ValidationResult(false, ErrorMessage);
            }

            if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
            {
                return new ValidationResult(false, ErrorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
