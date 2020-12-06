using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TasksApi.Handlers.Pipelines
{
    public class CustomPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Console.WriteLine("-- Handling Request");
            var response = await next();
            Console.WriteLine("-- Finished Request");
            return response;
        }
    }
}