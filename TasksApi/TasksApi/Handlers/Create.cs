using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models;
using Tasks.DataLayer.Models.Enums;
using TasksApi.Resources;

namespace TasksApi.Handlers
{
    public static class Create
    {
        public class Command : IRequest<TaskDetails>
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public int Priority { get; set; }

            public int TimeToComplete { get; set; }
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
                var taskModel = new TaskModel
                {
                    Name = request.Name,
                    Description = request.Description,
                    Priority = request.Priority,
                    TimeToComplete = request.TimeToComplete,
                    Status = TaskModelStatus.Active
                };

                _context.Tasks.Add(taskModel);

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<TaskDetails>(taskModel);
            }
        }
    }
}
