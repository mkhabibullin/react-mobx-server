using MediatR;
using TimeReport.Dto;

namespace TimeReport.Queries
{
    public class GetTimeReportQuery : IRequest<TimeReportDto>
    {
        public string Url { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }
    }
}
