using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TimeReport.Dto;
using TimeReport.Services;

namespace TimeReport.Queries
{
    public class GetTimeReportQueryHandler : IRequestHandler<GetTimeReportQuery, TimeReportDto>
    {
        IParseJiraTimeReport _parseJiraTimeReport;

        public GetTimeReportQueryHandler(IParseJiraTimeReport parseJiraTimeReport)
        {
            _parseJiraTimeReport = parseJiraTimeReport;
        }

        public async Task<TimeReportDto> Handle(GetTimeReportQuery request, CancellationToken cancellationToken)
        {
            return _parseJiraTimeReport.GetReportByLink(request.Url, request.Email, request.Pass);
        }
    }
}
