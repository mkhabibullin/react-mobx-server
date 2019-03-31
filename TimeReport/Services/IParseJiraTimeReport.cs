using TimeReport.Dto;

namespace TimeReport.Services
{
    public interface IParseJiraTimeReport
    {
        TimeReportDto GetReportByLink(string url, string email, string pass);
    }
}
