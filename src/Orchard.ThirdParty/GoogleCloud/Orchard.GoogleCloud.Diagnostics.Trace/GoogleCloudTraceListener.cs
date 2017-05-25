using System;
using System.Collections.Generic;
using Google.Cloud.Diagnostics.Common;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class GoogleCloudTraceListener : IDisposable
    {
        private readonly IManagedTracer _tracer;

        private readonly Stack<Tuple<string, IDisposable>> _runningSpans
            = new Stack<Tuple<string, IDisposable>>();

        public GoogleCloudTraceListener(
            IManagedTracer tracer)
        {
            _tracer = tracer;
        }

        [DiagnosticName("Orchard.GoogleCloud.Diagnostics.TraceStarted")]
        public virtual void OnTraceStarting(
            string name
            )
        {
            _runningSpans.Push(new Tuple<string, IDisposable>(name, _tracer.StartSpan(name)));
        }

        [DiagnosticName("Orchard.GoogleCloud.Diagnostics.TraceFinished")]
        public virtual void OnTraceFinished()
        {
            _runningSpans.Pop().Item2.Dispose();
        }

        public void Dispose()
        {
            foreach (var span in _runningSpans)
            {
                span.Item2.Dispose();
            }
        }
    }
}
