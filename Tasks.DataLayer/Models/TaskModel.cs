using System;
using Tasks.DataLayer.Interfaces;
using Tasks.DataLayer.Models.Base;
using Tasks.DataLayer.Models.Enums;

namespace Tasks.DataLayer.Models
{
    /// <summary>
    /// Represents task model.
    /// </summary>
    public class TaskModel : TimeTrackableEntityWithId, ISoftDeletable
    {
        /// <summary>
        /// Task Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Task Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Task description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Task priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Task time to complete in seconds.
        /// </summary>
        public int TimeToComplete { get; set; }

        /// <summary>
        /// Task status.
        /// </summary>
        public TaskModelStatus Status { get; set; }
    }
}
