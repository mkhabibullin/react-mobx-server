using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeReport.Dto;
using TimeReport.Extensions;
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
                .GetTimeTrackingByLink(request.Url, request.Email, request.Pass);

            Func<TimeTrackingTaskItemDto, bool> itemsFilter = ti => request.DateFrom <= ti.Date && ti.Date <= request.DateTo;

            var items = timeTracking
                .Tasks
                .Where(t => t.Itmes.Any(itemsFilter))
                .Select(t => new TimeReportItemDto(
                    t.Name, 
                    t.Link, 
                    t.Itmes.Where(itemsFilter).Aggregate(0f, (sum, i) => sum + i.TimeSpent.ParseFloat())))
                .ToArray();

            return items;
        }
    }
}
