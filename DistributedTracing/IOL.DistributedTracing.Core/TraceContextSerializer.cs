using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOL.DistributedTracing.Core
{
    internal class TraceContextSerializer
    {

        private static readonly TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;

        internal static string Serialize(Activity activity)
        {
            var dictionary = new Dictionary<string, string>();
            _propagator.Inject(new PropagationContext(activity.Context, OpenTelemetry.Baggage.Current),
                dictionary,
                (container, key, value) => container[key] = value);
            return System.Text.Json.JsonSerializer.Serialize(dictionary, dictionary.GetType());
        }

        internal static ActivityContext Deserialize(string data)
        {
            var dictionary = (Dictionary<string,string>)System.Text.Json.JsonSerializer.Deserialize(data, typeof(Dictionary<string, string>));
            var parentContext = _propagator.Extract(default, dictionary, ExtractTraceFromProperties);
            OpenTelemetry.Baggage.Current = parentContext.Baggage;
            return parentContext.ActivityContext;
        }

        private static List<string> ExtractTraceFromProperties(Dictionary<string, string> contextDictionary, string key)
        {
            var result = new List<string>();

            if (contextDictionary.TryGetValue(key, out var value))
                result.Add(value);

            return result;
        }
    }
}
