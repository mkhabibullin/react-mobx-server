using AutoMapper;
using System;
using TimeReport.Dto;
using TimeReport.Dto.Jira;

namespace TimeReport.Profiles
{
    public class TimeTrackingProfile : Profile
    {
        public TimeTrackingProfile()
        {
            CreateMap<JiraWorkLogItemDto, TimeTrackingTaskItemDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Created)));
        }
    }
}
