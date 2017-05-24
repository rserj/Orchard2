using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Trace.V1;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class GoogleCloudTraceListener
    {
        private readonly TraceServiceClient _client;

        public GoogleCloudTraceListener(
            TraceServiceClient client)
        {
            _client = client;
        }

        [DiagnosticName("Trace.Starting")]
        public virtual Task OnTraceStarting()
        {
            return null;
        }

        [DiagnosticName("Trace.Finished")]
        public virtual Task OnTraceFinished()
        {
            var trace = new Google.Cloud.Trace.V1.Trace { };
            trace.Spans.Add(new TraceSpan());

            var traces = new Traces();
            traces.Traces_.Add(trace);

            return _client.PatchTracesAsync(new PatchTracesRequest
            {
                Traces = traces
            });
        }
    }
}
