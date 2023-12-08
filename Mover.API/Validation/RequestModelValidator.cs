using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Mover.API.Validation
{
    public static class RequestModelValidator
    {
        public static void Validate<T>(T requestModel)
        {
            if (requestModel == null)
            {
                var errorMessage = "Request model is null.";
                var validationException = new ValidationException(errorMessage);
                Log.Error(validationException, errorMessage);
                throw validationException;
            }

            var validationContext = new ValidationContext(requestModel, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(requestModel, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                var errorMessage = $"Invalid request model. Validation errors: {string.Join(", ", validationErrors)}";
                var validationException = new ValidationException(errorMessage);
                Log.Error(validationException, errorMessage);
                throw validationException;
            }
        }
    }
}
