namespace TimeReport.Dto
{
    public class TimeReportItemDto
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public float Hours { get; set; }

        public TimeReportItemDto(string name, string link, float hours)
        {
            Name = name;
            Link = link;
            Hours = hours;
        }
    }
}
