using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tasks.DataLayer.EfClasses;
using TasksApi.Resources;

namespace TasksApi.Handlers
{
    public static class Get
    {
        public class Query : IRequest<TaskDetails>
        {
            [FromRoute]
            public Guid TaskId { get; set; }
        }

        public class Handler : IRequestHandler<Query,TaskDetails>
        {
            private readonly TasksDbContext _context;
            private readonly IMapper _mapper;

            public Handler(TasksDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TaskDetails> Handle(Query request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.FindAsync(request.TaskId);
                return _mapper.Map<TaskDetails>(task);
            }
        }
    }
}
