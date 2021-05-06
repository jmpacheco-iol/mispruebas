using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IOL.DistributedTracing.Core
{
    public class Trace
    {

        private readonly Activity _activity;

        internal Trace(Activity activity)
        {
            _activity = activity;
        }

        public void SaveProperty(string name, object value)
        {
            _activity.SetTag(name, value);
        }

        public string GetContextData()
        {
            return TraceContextSerializer.Serialize(_activity);
        }

    }
}
