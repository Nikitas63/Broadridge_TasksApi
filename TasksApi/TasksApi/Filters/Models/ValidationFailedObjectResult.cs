using Microsoft.AspNetCore.Http;
using TasksApi.Errors;

namespace TasksApi.Filters.Models
{
    public class ValidationFailedObjectResult : ErrorResponseResult
    {
        public ValidationFailedObjectResult(
            ErrorResponse value,
            int statusCode = StatusCodes.Status400BadRequest)
            : base(value)
        {
            value.Code = statusCode;
            StatusCode = statusCode;
        }
    }
}
