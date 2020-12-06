using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models;
using TasksApi.Controllers;
using TasksApi.Handlers;
using TasksApi.Resources;
using TasksApi.Validation;

namespace TasksApi.Validators
{
    public static class Validators
    {
        public class DeleteCommandValidator : AbstractValidator<Delete.Command>
        {
            public DeleteCommandValidator(TasksDbContext context)
            {
                RuleFor(m => m.TaskId).MustFindEntityById<Delete.Command, TaskModel>(context, StatusCodes.Status404NotFound);
            }
        }
        
        public class GetQueryValidator : AbstractValidator<Get.Query>
        {
            public GetQueryValidator(TasksDbContext context)
            {
                RuleFor(c => c.TaskId)
                    .NotEmpty();

                RuleFor(m => m.TaskId).MustFindEntityById<Get.Query, TaskModel>(context, StatusCodes.Status404NotFound);
            }

            private bool BeAValidOrder(string includeValue) =>
                Enum.TryParse(typeof(SortingFields), includeValue, true, out var val);
        }
        
        public class ListQueryValidator : AbstractValidator<List.Query>
        {
            public ListQueryValidator()
            {
                When(m => m.Page != null, () => { RuleFor(m => m.Page).GreaterThan(0); });
                When(m => m.Size != null, () => { RuleFor(m => m.Size).GreaterThan(0); });

                RuleForEach(m => m.OrderAsc).Must(BeAValidOrder);
                RuleForEach(m => m.OrderDesc).Must(BeAValidOrder);

                RuleFor(m => m.Filter)
                    .Must(BeAValidFilter)
                    .When(m => !string.IsNullOrWhiteSpace(m.Filter));
            }

            private bool BeAValidOrder(string includeValue) =>
                Enum.TryParse(typeof(SortingFields), includeValue, true, out var val);

            private bool BeAValidFilter(string includeValue) =>
                Enum.TryParse(typeof(FilterProperties), includeValue, true, out var val);
        }
        
        public class PostCommandValidator : AbstractValidator<Create.Command>
        {
            public PostCommandValidator()
            {
                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.Description)
                    .NotEmpty();

                RuleFor(c => c.Priority)
                    .GreaterThan(0);

                RuleFor(c => c.TimeToComplete)
                    .GreaterThan(0);
            }
        }
        
        public class PutCommandValidator : AbstractValidator<Update.Command>
        {
            public PutCommandValidator(TasksDbContext context)
            {
                RuleFor(c => c.TaskId)
                    .NotEmpty();

                RuleFor(c => c.Status)
                    .NotEmpty();

                RuleFor(m => m.TaskId).MustFindEntityById<Update.Command, TaskModel>(context, StatusCodes.Status404NotFound);
            }
        }
    }
}