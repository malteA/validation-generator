using System.ComponentModel.DataAnnotations;

namespace Project.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FirstDayOfMonthValidation : ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"Day muste be the first of the month";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime == false)
            {
                return new ValidationResult(GetErrorMessage());
            }
            DateTime dtValue = (DateTime)value;
            if (dtValue.Day != 1)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
