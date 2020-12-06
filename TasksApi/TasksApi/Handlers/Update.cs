using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models.Enums;
using TasksApi.Resources;

namespace TasksApi.Handlers
{
    public static class Update
    {
        public class Command : IRequest<TaskDetails>
        {
            [FromRoute]
            public Guid TaskId { get; set; }

            /// <summary>
            /// Task status: 1 - Active, 2 - Completed
            /// </summary>
            public TaskModelStatus Status { get; set; }
        }

        public class Handler : IRequestHandler<Command, TaskDetails>
        {
            private readonly TasksDbContext _context;
            private readonly IMapper _mapper;

            public Handler(TasksDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TaskDetails> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.FindAsync(request.TaskId);

                task.Status = request.Status;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<TaskDetails>(task);
            }
        }
    }
}
