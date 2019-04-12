using MediatR;
using System;
using System.Linq;
using System.Text.RegularExpressions;
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

            Func<TimeTrackingTaskItemDto, bool> itemsFilter = ti => request.DateFrom.Date <= ti.Date.Date && ti.Date.Date <= request.DateTo.Date;

            var items = timeTracking
                .Tasks
                .Where(t => t.Itmes.Any(itemsFilter))
                .Select(t => new TimeReportItemDto(
                    t.Name, 
                    t.Link, 
                    t.Itmes.Where(itemsFilter).Aggregate(0f, (sum, i) => {
                        var result = sum;

                        var timePartsMatches = Regex.Matches(i.TimeSpent, @"\d+ \w+");

                        if (timePartsMatches.Any())
                        {
                            foreach (var m in timePartsMatches)
                            {
                                var v = m.ToString();
                                if(!string.IsNullOrWhiteSpace(v))
                                {
                                    if(v.Contains("min"))
                                    {
                                        result += v.ParseFloat() / 60;
                                    }
                                    else
                                    {
                                        result += v.ParseFloat();
                                    }
                                }
                            }
                        }
                        else
                        {
                            result += i.TimeSpent.ParseFloat();
                        }

                        return result;
                    })))
                .ToArray();

            return items;
        }
    }
}
