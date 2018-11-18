using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models.Base;
using TasksApi.Filters.Models;

namespace TasksApi.Validation
{
    /// <summary>
    /// Helper methods for validation.
    /// </summary>
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<TRequest, Guid> MustFindEntityById<TRequest, TEntityType>(
            this IRuleBuilder<TRequest, Guid> ruleBuilder,
            TasksDbContext context,
            int statusCode = StatusCodes.Status400BadRequest)
            where TEntityType : EntityWithId
        {
            return ruleBuilder.SetValidator(new MustFindTask<TEntityType>(context))
                .WithMessageAndStatusCode($"Cannot find {typeof(TEntityType).Name} task with Id = {{PropertyValue}}", statusCode);
        }

        public static IRuleBuilderOptions<TRequest, TProperty> WithMessageAndStatusCode<TRequest, TProperty>(
            this IRuleBuilderOptions<TRequest, TProperty> ruleBuilderOptions,
            string message,
            int statusCode)
        {
            return ruleBuilderOptions.WithMessage(
                r => JsonConvert.SerializeObject(
                    new ValidationCustomErrorAsMessage { StatusCode = statusCode, Message = message }, Newtonsoft.Json.Formatting.None));
        }
    }
}
