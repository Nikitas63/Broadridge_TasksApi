using System;

namespace Tasks.DataLayer.Interfaces
{
    public interface ITimeTrackable
    {
        DateTimeOffset CreatedDate { get; set; }

        DateTimeOffset UpdatedDate { get; set; }
    }
}

