using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace TourAgencyApp
{
    public class IntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "Value cannot be empty.");
            }

            if (int.TryParse(value as string, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid int number.");
            }
        }
    }
}
