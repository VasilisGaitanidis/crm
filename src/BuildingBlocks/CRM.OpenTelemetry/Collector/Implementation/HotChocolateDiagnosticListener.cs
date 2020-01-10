using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using OpenTelemetry.Collector;
using OpenTelemetry.Trace;

namespace CRM.OpenTelemetry.Collector.Implementation
{
    internal class HotChocolateDiagnosticListener : ListenerHandler
    {
        private readonly PropertyFetcher _startContextFetcher = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher _stopContextFetcher = new PropertyFetcher("HttpContext");

        public HotChocolateDiagnosticListener(string sourceName, Tracer tracer) : base(sourceName, tracer)
        {
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            var operationName = activity.OperationName;
            const string EventNameSuffix = ".OnStartActivity";

            if (operationName == "Microsoft.AspNetCore.Hosting.HttpRequestIn")
            {
                if (!(_startContextFetcher.Fetch(payload) is HttpContext context))
                {
                    CollectorEventSource.Log.NullPayload(nameof(HotChocolateDiagnosticListener) + EventNameSuffix);
                    return;
                }
                if (context.Request.Path != "/graphql")
                {
                    CollectorEventSource.Log.RequestIsFilteredOut(activity.OperationName);
                    return;
                }
                this.Tracer.StartActiveSpanFromActivity(context.Request.Path, activity, SpanKind.Server, out var span);

                if (span.IsRecording)
                {
                    span.PutHttpHostAttribute(context.Request.Host.Host, context.Request.Host.Port ?? 80);
                    span.PutHttpMethodAttribute(context.Request.Method);
                    span.PutHttpPathAttribute(context.Request.Path);

                    var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
                    span.PutHttpUserAgentAttribute(userAgent);
                }
            }
            else if (operationName == "HotChocolate.Execution.Query")
            {
                var property = payload.GetType()
                    .GetTypeInfo()
                    .DeclaredProperties.FirstOrDefault(p => string.Equals(p.Name, "context", StringComparison.InvariantCultureIgnoreCase));

                if (property != null)
                {
                    var context = property.GetValue(payload, null) as HotChocolate.Execution.QueryContext;

                    this.Tracer.StartActiveSpanFromActivity(operationName, activity, SpanKind.Server, out var span);

                    if (span.IsRecording)
                    {
                        span.SetAttribute("query", context.Request.Query.ToString());
                        span.PutHttpMethodAttribute("POST");
                        span.SetAttribute("operation", context.Request.OperationName);
                    }
                }
            }
        }

        public override void OnStopActivity(Activity activity, object payload)
        {
            const string EventNameSuffix = ".OnStopActivity";

            var span = this.Tracer.CurrentSpan;

            if (span == null || !span.Context.IsValid)
            {
                CollectorEventSource.Log.NullOrBlankSpan(nameof(HotChocolateDiagnosticListener) + EventNameSuffix);
                return;
            }

            if (span.IsRecording)
            {
                var operationName = activity.OperationName;
                if (operationName == "Microsoft.AspNetCore.Hosting.HttpRequestIn")
                {
                    if (!(_stopContextFetcher.Fetch(payload) is HttpContext context))
                    {
                        CollectorEventSource.Log.NullPayload(nameof(HotChocolateDiagnosticListener) + EventNameSuffix);
                        return;
                    }

                    var response = context.Response;
                    span.PutHttpStatusCode(response.StatusCode, response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase);
                }
            }

            span.End();
        }
    }
}