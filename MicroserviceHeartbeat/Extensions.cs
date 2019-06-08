using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroserviceHeartbeat
{
    public static class Extensions
    {
        public static IMvcBuilder AddMicrosoftHeartbeat(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetExecutingAssembly());
        }
    }
}
