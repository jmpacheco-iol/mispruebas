using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOL.DistributedTracing.Core
{
    public class TraceCategory
    {

        private readonly ActivityKind _activityKind;
        private static readonly ConcurrentDictionary<ActivityKind, object> _categories;

        static TraceCategory()
        {
            _categories = new ConcurrentDictionary<ActivityKind, object>();
        }

        private TraceCategory(ActivityKind activityKind)
        {
            _activityKind = activityKind;
        }

        internal ActivityKind ActivityKind { get { return _activityKind; } }

        public static TraceCategory Producer { get { return GetTraceCategory(ActivityKind.Producer); } }
        public static TraceCategory Consumer { get { return GetTraceCategory(ActivityKind.Consumer); } }
        public static TraceCategory Server { get { return GetTraceCategory(ActivityKind.Server); } }
        public static TraceCategory Client { get { return GetTraceCategory(ActivityKind.Client); } }
        public static TraceCategory Internal { get { return GetTraceCategory(ActivityKind.Internal); } }

        private static TraceCategory GetTraceCategory(ActivityKind activityKind)
        {
            return (TraceCategory)_categories.GetOrAdd(activityKind, kind => new TraceCategory(kind));
        }

    }
}
