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

        public JiraWorkLogItemCommentDto Comment { get; set; }
    }

    public class JiraWorkLogItemCommentDto
    {
        public JiraWorkLogItemCommentContentDto[] Content { get; set; }

        public class JiraWorkLogItemCommentContentDto
        {
            public JiraWorkLogItemCommentSubContentDto[] Content { get; set; }
        }

        public class JiraWorkLogItemCommentSubContentDto
        {
            public string Text { get; set; }
        }
    }
}
