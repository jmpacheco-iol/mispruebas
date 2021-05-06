using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOL.DistributedTracing.Core
{
    public class TraceManager
    {

        private readonly TracerProvider _tracerProvider;
        private readonly string _name;

        internal TraceManager(TracerProvider tracerProvider, string name)
        {
            _tracerProvider = tracerProvider;
            _name = name;
            Current = this;
        }

        internal string Name { get { return _name; } }

        internal static TraceManager Current { get; private set; }
    }
}
