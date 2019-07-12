using AutoMapper;
using NSpecifications;
using static TimeReport.Profiles.TimeTrackingProfile;

namespace TimeReport.Extensions
{
    public static class AutoMapperExtensions
    {
        public static T MapWithSpec<T, S>(this IMapper mapper, object source, Spec<S> spec)
        {
            return mapper.Map<T>(source, opt => opt.Items[SpecOption] = spec);
        }
    }
}
