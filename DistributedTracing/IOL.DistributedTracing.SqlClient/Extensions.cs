using IOL.DistributedTracing.Core;
using OpenTelemetry.Instrumentation.SqlClient;
using OpenTelemetry.Trace;

namespace IOL.DistributedTracing.SqlClient
{
    public static class Extensions
    {
        public static TraceManagerBuilder AddAspNetCoreTracing(this TraceManagerBuilder builder)
        {
            builder.AddAction(nativeBuilder => nativeBuilder.AddSqlClientInstrumentation(AddInstrumentation));
            return builder;
        }

        private static void AddInstrumentation(SqlClientInstrumentationOptions options)
        {
            options.RecordException = true;
            options.SetDbStatementForText = true;
        }
    }
}
