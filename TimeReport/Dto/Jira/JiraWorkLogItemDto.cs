using System.Collections.Generic;

namespace TimeReport.Dto.Jira
{
    public class JiraWorkLogsDto
    {
        public IEnumerable<JiraWorkLogItemDto> Worklogs { get; set; }
    }

    public class JiraWorkLogItemDto
    {
        public string Created { get; set; }

        public int TimeSpentSeconds { get; set; }
    }
}
