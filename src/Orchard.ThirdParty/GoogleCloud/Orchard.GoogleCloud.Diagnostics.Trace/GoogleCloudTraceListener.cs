using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Diagnostics.Common;
using Google.Cloud.Trace.V1;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class GoogleCloudTraceListener
    {
        private readonly IManagedTracer _tracer;

        public GoogleCloudTraceListener(
            IManagedTracer tracer)
        {
            _tracer = tracer;
        }

        [DiagnosticName("Trace.Starting")]
        public virtual Task OnTraceStarting()
        {
            return null;
        }

        [DiagnosticName("Trace.Finished")]
        public virtual Task OnTraceFinished(
            string name,
            string projectId,
            IDictionary<string,string> labels,
            DateTimeOffset startTime,
            DateTimeOffset endTime
            )
        {
            // https://github.com/GoogleCloudPlatform/google-cloud-dotnet/blob/master/apis/Google.Cloud.Diagnostics.AspNetCore/Google.Cloud.Diagnostics.AspNetCore.Snippets/AspNetCoreSnippets.cs

            return null;

            //var trace = new Google.Cloud.Trace.V1.Trace {
            //    /*TraceId = ?*/
            //    ProjectId = projectId,
            //};

            //var traceSpan = new TraceSpan
            //{
            //    Name = name,
            //    /*ParentSpanId = ?*/
            //    /*SpanId = ?*/
            //    StartTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(startTime),
            //    EndTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(endTime),
            //};

            //traceSpan.Labels.Add(labels);

            //trace.Spans.Add(traceSpan);

            //var traces = new Traces();
            //traces.Traces_.Add(trace);


            //return _client.PatchTracesAsync(new PatchTracesRequest
            //{
            //    Traces = traces
            //});
        }
    }
}
