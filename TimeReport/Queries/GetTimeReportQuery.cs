using MediatR;
using System;
using TimeReport.Dto;

namespace TimeReport.Queries
{
    public class GetTimeReportQuery : IRequest<TimeReportItemDto[]>
    {
        public string Url { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
