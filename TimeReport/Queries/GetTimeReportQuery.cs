using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using TimeReport.Dto;

namespace TimeReport.Queries
{
    public class GetTimeReportQuery : IRequest<TimeReportItemDto[]>
    {
        [DataType(DataType.Url)]
        public string Url { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Pass { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
