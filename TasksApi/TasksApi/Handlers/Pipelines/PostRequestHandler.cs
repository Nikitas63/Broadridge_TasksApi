using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace TasksApi.Handlers.Pipelines
{
    public class PostRequestHandler<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>  
        where TRequest : class
        where TResponse : class
    {
        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            Console.WriteLine("- Request Done");
            return Task.CompletedTask;
        }
    }
}
