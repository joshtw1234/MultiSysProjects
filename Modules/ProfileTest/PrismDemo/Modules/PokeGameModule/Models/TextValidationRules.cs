using System;
using System.Globalization;
using System.Windows.Controls;

namespace PokeGameModule.Models
{
    public class TextValidationRules : ValidationRule
    {
        private const string InvalidInput = "Please enter valid number!";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            float val;
            if (!string.IsNullOrEmpty((string)value))
            {
                // Validates weather Non numeric values are entered as the Age
                if (!float.TryParse(value.ToString(), out val))
                {
                    return new ValidationResult(false, InvalidInput);
                }
            }

            return new ValidationResult(true, null);
        }
    }
}
