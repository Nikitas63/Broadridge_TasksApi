using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using TasksApi.Errors;

namespace TasksApi.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;

        public HttpGlobalExceptionFilter(
            IHostingEnvironment env,
            ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var errorResponse = new ErrorResponse();


            errorResponse.Details = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Source = context.Exception.GetType().ToString(),
                        Message = context.Exception.ToString()
                    }
                };

            context.ExceptionHandled = true;
        }
    }
}
