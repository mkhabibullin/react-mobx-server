using MediatR;
using NSpecifications;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeReport.Dto;
using TimeReport.Services;

namespace TimeReport.Queries
{
    public class GetTimeReportQueryHandler : IRequestHandler<GetTimeReportQuery, TimeReportItemDto[]>
    {
        IParseJiraTimeReport _parseJiraTimeReport;

        public GetTimeReportQueryHandler(IParseJiraTimeReport parseJiraTimeReport)
        {
            _parseJiraTimeReport = parseJiraTimeReport;
        }

        public async Task<TimeReportItemDto[]> Handle(GetTimeReportQuery request, CancellationToken cancellationToken)
        {
            var timeTracking = _parseJiraTimeReport
                .GetTimeTrackingByLink(request.Url, request.Email, request.Pass, request.DateFrom, request.DateTo);

            var itemsIsActualSpec = 
                new Spec<TimeTrackingTaskItemDto>(ti => request.DateFrom.Date <= ti.Date.Date && ti.Date.Date <= request.DateTo.Date);

            var items = timeTracking
                .Tasks
                .Where(t => t.Itmes.Any(itemsIsActualSpec))
                .Select(t => new TimeReportItemDto(
                    t.Name, 
                    t.Link, 
                    t.Itmes.Where(itemsIsActualSpec).Aggregate(0f, (sum, i) => sum + (i.TimeSpentSeconds / 60f / 60f))
                ))
                .ToArray();

            return items;
        }
    }
}
