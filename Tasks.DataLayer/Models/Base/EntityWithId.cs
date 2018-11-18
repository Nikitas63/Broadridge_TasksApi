using System;

namespace Tasks.DataLayer.Models.Base
{
    public abstract class EntityWithId : Entity, IEntityKeyProvider
    {
        /// <summary>
        /// Primary Key of the entity.
        /// </summary>
        public Guid Id { get; set; }

        public object[] GetKeyValues()
        {
            return new object[] { Id };
        }
    }
}
