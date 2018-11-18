﻿using System;
using System.Threading.Tasks;
using Tasks.DataLayer.Interfaces;
using Tasks.DataLayer.Models.Base;
using Tasks.DataLayer.Models.Enums;

namespace Tasks.DataLayer.Models
{
    public class TaskModel : TimeTrackableEntityWithId, ISoftDeletable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        /// <summary>
        /// In seconds
        /// </summary>
        public int TimeToComplete { get; set; }

        public TaskModelStatus Status { get; set; }
    }
}
