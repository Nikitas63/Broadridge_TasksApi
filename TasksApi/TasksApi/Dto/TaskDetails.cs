using System;
using Tasks.DataLayer.Models.Enums;

namespace TasksApi.Dto
{
    public class TaskDetails
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public int TimeToComplete { get; set; }

        public TaskModelStatus Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
