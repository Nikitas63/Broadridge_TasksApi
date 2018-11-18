using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models;
using TasksApi.Dto;
using TasksApi.Pagination;

namespace TasksApi.Controllers
{
    public static class List
    {
        public class Query : IRequest<Page<TaskDetails>>
        {
            public Query()
            {
                this.Page = 1;
                this.Size = 20;
            }

            /// <summary>
            /// Page number (for paginated response)
            /// </summary>
            public int? Page { get; set; }

            /// <summary>
            /// Page size (for paginated response)
            /// </summary>
            public int? Size { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                When(m => m.Page != null, () => { RuleFor(m => m.Page).GreaterThan(0); });
                When(m => m.Size != null, () => { RuleFor(m => m.Size).GreaterThan(0); });
            }
        }

        public class Handler : IRequestHandler<Query, Page<TaskDetails>>
        {
            private readonly TasksDbContext _context;

            public Handler(TasksDbContext context)
            {
                _context = context;
            }

            public async Task<Page<TaskDetails>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<TaskModel> query = _context.Tasks;

                return await query
                    .OrderBy(t => t.Priority)
                    .ProjectToPageAsync<TaskDetails>(request.Page ?? 1, request.Size ?? 20);
            }
        }
    }
}
