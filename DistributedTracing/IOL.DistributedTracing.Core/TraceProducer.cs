using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace IOL.DistributedTracing.Core
{
    public class TraceProducer
    {

        private static readonly ConcurrentDictionary<string, ActivitySource> _sourcesDictionary;

        static TraceProducer()
        {
            _sourcesDictionary = new ConcurrentDictionary<string, ActivitySource>();
        }

        public static Trace StartNewTrace(string traceName, TraceCategory traceCategory = null, string sourceName = null)
        {
            var activitySource = ResolveSource(sourceName);
            return new Trace(activitySource.StartActivity(traceName, ResolveKind(traceCategory)));
        }

        public static Trace StartInnerTrace(string parentData, string traceName, TraceCategory traceCategory = null, string sourceName = null)
        {            
            var parentContext = TraceContextSerializer.Deserialize(parentData);
            var activitySource = ResolveSource(sourceName);
            return new Trace(activitySource.StartActivity(traceName, ResolveKind(traceCategory), parentContext));
        }

        private static ActivitySource ResolveSource(string sourceName)
        {
            var source = sourceName ?? TraceManager.Current.Name;
            return _sourcesDictionary.GetOrAdd(source, name => new ActivitySource(name));
        }

        private static ActivityKind ResolveKind(TraceCategory traceCategory)
        {
            return (traceCategory ?? TraceCategory.Internal).ActivityKind;
        }

    }
}
