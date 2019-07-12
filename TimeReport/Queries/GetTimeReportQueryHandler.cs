using AutoMapper;
using MediatR;
using NSpecifications;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeReport.Dto;
using TimeReport.Services;
using TimeReport.Extensions;

namespace TimeReport.Queries
{
    public class GetTimeReportQueryHandler : IRequestHandler<GetTimeReportQuery, TimeReportItemDto[]>
    {
        private readonly IParseJiraTimeReport _parseJiraTimeReport;
        private readonly IMapper _mapper;

        public GetTimeReportQueryHandler(IParseJiraTimeReport parseJiraTimeReport, IMapper mapper)
        {
            _parseJiraTimeReport = parseJiraTimeReport;
            _mapper = mapper;
        }

        public async Task<TimeReportItemDto[]> Handle(GetTimeReportQuery request, CancellationToken cancellationToken)
        {
            var timeTracking = _parseJiraTimeReport
                .GetTimeTrackingByLink(request.Url, request.Email, request.Pass, request.DateFrom, request.DateTo);

            var actualItemsSpec = 
                new Spec<TimeTrackingTaskItemDto>(ti => request.DateFrom.Date <= ti.Date.Date && ti.Date.Date <= request.DateTo.Date);

            var items = timeTracking
                .Tasks
                .Where(t => t.Itmes.Any(actualItemsSpec))
                .Select(t => _mapper.MapWithSpec<TimeReportItemDto, TimeTrackingTaskItemDto>(t, actualItemsSpec))
                .ToArray();

            return items;
        }
    }
}
