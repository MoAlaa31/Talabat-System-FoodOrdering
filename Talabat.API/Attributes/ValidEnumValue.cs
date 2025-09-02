using System.ComponentModel.DataAnnotations;

namespace Talabat.API.Attributes
{
    public class ValidEnumValueAttribute<T> : ValidationAttribute where T : Enum
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
                return ValidationResult.Success;

            if(Enum.IsDefined(typeof(T), value))
                return ValidationResult.Success;

            return new ValidationResult($"Invalid value for {validationContext.DisplayName}.");
        }
    }
    
}
