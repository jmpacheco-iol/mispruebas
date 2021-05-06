using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOL.DistributedTracing.Core
{
    public class TraceManagerBuilder
    {

        private readonly string _traceManagerName;
        private readonly List<Action<TracerProviderBuilder>> _actions;

        public TraceManagerBuilder(string traceManagerName, string dataCollectorUrl)
        {
            _actions = new List<Action<TracerProviderBuilder>>();

            AddAction(builder => builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(traceManagerName)));

            AddAction(builder => builder.AddJaegerExporter(opts =>
              {
                  var dataCollectorUrlParts = dataCollectorUrl.Split(':');
                  opts.AgentHost = dataCollectorUrlParts[0];
                  opts.AgentPort = Convert.ToInt32(dataCollectorUrlParts[1]);
              }));

            AddSources(traceManagerName);
            _traceManagerName = traceManagerName;
        }

        public TraceManagerBuilder AddAction(Action<TracerProviderBuilder> action)
        {
            _actions.Add(action);
            return this;
        }

        public TraceManagerBuilder AddConsoleDataCollector()
        {
            AddAction(builder => builder.AddConsoleExporter());
            return this;
        }

        public TraceManagerBuilder AddSources(params string[] sources)
        {
            sources.ToList().ForEach(source => _actions.Add(builder => builder.AddSource(source)));
            return this;
        }

        public TraceManager Build()
        {
            var tracerProviderBuilder = OpenTelemetry.Sdk.CreateTracerProviderBuilder();
            RunAllActions(tracerProviderBuilder);
            return new TraceManager(tracerProviderBuilder.Build(), _traceManagerName);
        }

        internal void RunAllActions(TracerProviderBuilder builder)
        {
            foreach (Action<TracerProviderBuilder> action in _actions)
            {
                action(builder);
            }
        }
    }
}
