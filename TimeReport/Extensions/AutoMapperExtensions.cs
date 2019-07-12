using AutoMapper;
using NSpecifications;

namespace TimeReport.Extensions
{
    public static class AutoMapperExtensions
    {
        public static string SpecOption => "Spec";

        public static T MapWithSpec<T, S>(this IMapper mapper, object source, Spec<S> spec)
        {
            return mapper.Map<T>(source, opt => opt.Items[SpecOption] = spec);
        }

        public static Spec<T> BySpecOf<T>(this ResolutionContext context)
            => context.Items[SpecOption] as Spec<T>;
    }
}
