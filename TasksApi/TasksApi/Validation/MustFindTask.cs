using System;
using FluentValidation.Validators;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models.Base;

namespace TasksApi.Validation
{
    internal class MustFindTask<TEntityType> : PropertyValidator
        where TEntityType : EntityWithId
    {
        private readonly TasksDbContext _context;

        internal MustFindTask(TasksDbContext context)
            : base(string.Empty)
        {
            _context = context;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return false;

            var entityId = (Guid)context.PropertyValue;

            var entity = _context.Find<TEntityType>(entityId);

            return entity != null;
        }
    }
}
