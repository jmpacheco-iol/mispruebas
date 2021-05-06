using IOL.DistributedTracing.Core;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Trace;

namespace IOL.DistributedTracing.AspNetCore
{
    public static class Extensions
    {
        public static TraceManagerBuilder AddAspNetCoreTracing(this TraceManagerBuilder builder)
        {
            builder.AddAction(nativeBuilder => nativeBuilder.AddAspNetCoreInstrumentation(AddInstrumentation));
            return builder;
        }

        private static void AddInstrumentation(AspNetCoreInstrumentationOptions options)
        {
            options.RecordException = true;
        }
    }
}
