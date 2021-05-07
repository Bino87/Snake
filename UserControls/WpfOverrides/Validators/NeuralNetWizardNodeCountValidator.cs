using System.Globalization;
using System.Windows.Controls;

namespace UserControls.WpfOverrides.Validators
{
    public class NeuralNetWizardNodeCountValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int i))
            {
                if(i > 0)
                    return ValidationResult.ValidResult;
                return new ValidationResult(false, $"Number of nodes must be 1 or higher, but was {i}");
            }

            return new ValidationResult(false, $"unable to parse {value} to number");
        }
    }
}
