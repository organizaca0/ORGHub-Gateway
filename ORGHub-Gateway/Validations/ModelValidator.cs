using System.ComponentModel.DataAnnotations;

namespace ORGHub_Gateway.Validations
{
    public class ModelValidator
    {
        public static string Validate<T>(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(model);
                var isRequired = Attribute.IsDefined(property, typeof(RequiredAttribute));

                if (isRequired && value == null)
                {
                    validationResults.Add(new ValidationResult($"{property.Name} is required.", new[] { property.Name }));
                }
            }

            if (validationResults.Count > 0)
            {
                var errors = validationResults.Select((result, index) => $"{index}: {string.Join(", ", result.MemberNames)}: {result.ErrorMessage}");
                return "Model Validation Error:\n" + string.Join("\n", errors);
            }

            return null;
        }

    }
}
