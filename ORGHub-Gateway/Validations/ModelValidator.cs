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
            var validationContext = new ValidationContext(model, null, null);

            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid)
            {
                Dictionary<int, string> errors = new Dictionary<int, string>();
                for (int i = 0; i < validationResults.Count; i++)
                {
                    var propertyNames = validationResults[i].MemberNames;
                    var propertyName = string.Join(", ", propertyNames);
                    var errorMessage = validationResults[i].ErrorMessage;

                    errors.Add(i, $"{propertyName}: {errorMessage}");
                }

                string errorString = "Model Validation Error:\n" + string.Join("\n", errors.Select(kvp => $"{kvp.Key}, {kvp.Value}"));

                return errorString;
            }

            return null;
        }
    }
}
