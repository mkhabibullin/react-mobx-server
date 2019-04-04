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
        public TimeTrackingTaskItemDto(DateTime date, string timeSpent, string comment)
        {
            Date = date;
            TimeSpent = timeSpent;
            Comment = comment;
        }

        public DateTime Date { get; set; }

        public string TimeSpent { get; set; }

        public string Comment { get; set; }
    }
}
