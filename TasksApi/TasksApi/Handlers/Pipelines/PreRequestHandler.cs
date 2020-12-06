using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace TasksApi.Handlers.Pipelines
{
    public class PreRequestHandler<TRequest> : IRequestPreProcessor<TRequest>  where TRequest : class
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine("- Request Start");
            return Task.CompletedTask;
        }
    }
}
