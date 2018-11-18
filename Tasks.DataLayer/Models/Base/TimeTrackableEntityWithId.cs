using System;
using Tasks.DataLayer.Interfaces;

namespace Tasks.DataLayer.Models.Base
{
    public abstract class TimeTrackableEntityWithId : EntityWithId, ITimeTrackable
    {
        /// <inheritdoc />
        public DateTimeOffset CreatedDate { get; set; }

        /// <inheritdoc />
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
