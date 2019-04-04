using TimeReport.Dto;

namespace TimeReport.Services
{
    public interface IParseJiraTimeReport
    {
        TimeTrackingDto GetTimeTrackingByLink(string url, string email, string pass);
    }
}
