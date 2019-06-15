using System;
using System.Collections.Generic;

namespace TimeReport.Dto
{
    public class TimeTrackingDto
    {
        public ICollection<TimeTrackingTaskDto> Tasks { get; set; } = new List<TimeTrackingTaskDto>();
    }

    public class TimeTrackingTaskDto
    {
        public TimeTrackingTaskDto(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public string Name { get; set; }

        public string Link { get; set; }

        public ICollection<TimeTrackingTaskItemDto> Itmes { get; set; } = new List<TimeTrackingTaskItemDto>();
    }

    public class TimeTrackingTaskItemDto
    {
        public TimeTrackingTaskItemDto(DateTime date, long timeSpentSeconds, string comment)
        {
            Date = date;
            TimeSpentSeconds = timeSpentSeconds;
            Comment = comment;
        }

        public DateTime Date { get; set; }

        public long TimeSpentSeconds { get; set; }

        public string Comment { get; set; }
    }
}
