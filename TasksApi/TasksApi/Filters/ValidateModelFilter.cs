using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TasksApi.Errors;
using TasksApi.Filters.Models;

namespace TasksApi.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
        {
            _logger = logger;
        }

        public static ValidationFailedObjectResult CreateValidationFailedObject(ModelStateDictionary modelState)
        {
            // Decomposite to our own error detail object
            var customValidationErrorDetails = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(e => new CustomValidationErrorDetail
                {
                    Source = key,
                    Message = ConvertToCustomErrorMessage(e.ErrorMessage)
                })).ToList();

            // Compose error response
            var errorResponse = new ErrorResponse
            {
                Message = "Validation failed",
                Details = customValidationErrorDetails.Select(x => new ErrorDetail
                {
                    Source = x.Source,
                    Message = x.Message.Message
                }).ToList()
            };

            int mergedStatusCode = CalculateMergedStatusCode(customValidationErrorDetails.Select(x => x.Message));

            return new ValidationFailedObjectResult(errorResponse, mergedStatusCode);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var validationFailedObject = CreateValidationFailedObject(context.ModelState);

            _logger.LogDebug($"Validation failed with result: {validationFailedObject}");

            context.Result = validationFailedObject;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static int CalculateMergedStatusCode(IEnumerable<ValidationCustomErrorAsMessage> errors)
        {
            var validationCustomErrorAsMessages = errors.ToList();

            if (validationCustomErrorAsMessages.FirstOrDefault(x => x.StatusCode == StatusCodes.Status404NotFound) != null)
                return StatusCodes.Status404NotFound;

            return validationCustomErrorAsMessages.FirstOrDefault()?.StatusCode ?? StatusCodes.Status400BadRequest;
        }

        private static ValidationCustomErrorAsMessage ConvertToCustomErrorMessage(string plainErrorMessage)
        {
            if (plainErrorMessage.StartsWith('{'))
            {
                // custom validation message (JSON object)
                var customError = JsonConvert.DeserializeObject<ValidationCustomErrorAsMessage>(plainErrorMessage);
                return customError;
            }

            return new ValidationCustomErrorAsMessage
            {
                Message = plainErrorMessage,
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        private class CustomValidationErrorDetail
        {
            public string Source { get; set; }

            public ValidationCustomErrorAsMessage Message { get; set; }
        }
    }
}
