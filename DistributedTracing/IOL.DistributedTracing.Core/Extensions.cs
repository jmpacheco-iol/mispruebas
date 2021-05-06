using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOL.DistributedTracing.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, TraceManagerBuilder builder)
        {
            return services.AddOpenTelemetryTracing(nativeBuilder => builder.RunAllActions(nativeBuilder));
        }
    }
}
