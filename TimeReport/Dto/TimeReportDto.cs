using System;
using System.Collections.Generic;

namespace TimeReport.Dto
{
    public class TimeReportDto
    {
        public ICollection<TimeReportTaskDto> Tasks { get; set; } = new List<TimeReportTaskDto>();
    }

    public class TimeReportTaskDto
    {
        public TimeReportTaskDto(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public string Name { get; set; }

        public string Link { get; set; }

        public ICollection<TimeReportTaskItemDto> Itmes { get; set; } = new List<TimeReportTaskItemDto>();
    }

    public class TimeReportTaskItemDto
    {
        public TimeReportTaskItemDto(DateTime date, string timeSpent, string comment)
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
