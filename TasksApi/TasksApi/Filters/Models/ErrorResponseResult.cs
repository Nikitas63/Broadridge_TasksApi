using Microsoft.AspNetCore.Mvc;
using TasksApi.Errors;

namespace TasksApi.Filters.Models
{
    public class ErrorResponseResult : ObjectResult
    {
        public ErrorResponseResult(ErrorResponse value)
            : base(value)
        {
        }
    }
}
