using AutoMapper;
using NSpecifications;
using System;
using System.Linq;
using TimeReport.Dto;
using TimeReport.Dto.Jira;

namespace TimeReport.Profiles
{
    public class TimeTrackingProfile : Profile
    {
        public static string SpecOption => "Spec";

        public TimeTrackingProfile()
        {
            CreateMap<JiraWorkLogItemDto, TimeTrackingTaskItemDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Created)));

            CreateMap<TimeTrackingTaskDto, TimeReportItemDto>()
                .ForMember(dest => dest.Hours, opt => opt.MapFrom((src, d, s, context) => 
                    src.Itmes.Where(context.Items[SpecOption] as Spec<TimeTrackingTaskItemDto>)
                    .Aggregate(0f, (sum, i) => sum + (i.TimeSpentSeconds / 60f / 60f))));
        }
    }
}
