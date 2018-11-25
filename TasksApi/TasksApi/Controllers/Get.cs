using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models;
using TasksApi.Dto;
using TasksApi.Validation;

namespace TasksApi.Controllers
{
    public static class Get
    {
        public class Query : IRequest<TaskDetails>
        {
            [FromRoute]
            public Guid TaskId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(TasksDbContext context)
            {
                RuleFor(c => c.TaskId)
                    .NotEmpty();

                RuleFor(m => m.TaskId).MustFindEntityById<Query, TaskModel>(context, StatusCodes.Status404NotFound);
            }

            private bool BeAValidOrder(string includeValue) =>
                Enum.TryParse(typeof(SortingFields), includeValue, true, out var val);
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
