using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tasks.DataLayer.EfClasses;

namespace TasksApi.Controllers
{
    public static class Delete
    {
        public class Command : IRequest
        {
            public Guid TaskId { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TasksDbContext _context;

            public Handler(TasksDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.FindAsync(request.TaskId);

                _context.Tasks.Remove(task);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
