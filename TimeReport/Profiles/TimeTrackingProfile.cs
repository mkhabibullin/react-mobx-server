using AutoMapper;
using System;
using System.Linq;
using TimeReport.Dto;
using TimeReport.Dto.Jira;
using TimeReport.Extensions;

namespace TimeReport.Profiles
{
    public class TimeTrackingProfile : Profile
    {
        public TimeTrackingProfile()
        {
            CreateMap<JiraWorkLogItemDto, TimeTrackingTaskItemDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Created)))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment != null ? string.Join(Environment.NewLine, src.Comment.Content.SelectMany(c => c.Content.Select(sc => sc.Text))) : ""));

            CreateMap<TimeTrackingTaskDto, TimeReportItemDto>()
                .ForMember(dest => dest.Hours, opt => opt.MapFrom((src, d, s, context) => src.Itmes
                    .Where(context.BySpecOf<TimeTrackingTaskItemDto>())
                    .Aggregate(0f, (sum, i) => sum + (i.TimeSpentSeconds / 60f / 60f))))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => string.Join(Environment.NewLine, src.Itmes.Select(i => i.Comment))));
        }
    }
}
