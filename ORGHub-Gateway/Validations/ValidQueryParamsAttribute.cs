using System.ComponentModel.DataAnnotations;

namespace ORGHub_Gateway.Validations
{
    public class ValidQueryParamsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is Dictionary<string, string> queryParams)
            {
                foreach (var key in queryParams.Keys)
                {
                    if (string.IsNullOrWhiteSpace(queryParams[key]))
                    {
                        return new ValidationResult($"Query parameter '{key}' must have a value.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
