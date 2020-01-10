using System;
using CRM.OpenTelemetry.Collector.Implementation;
using OpenTelemetry.Collector;
using OpenTelemetry.Trace;

namespace CRM.OpenTelemetry.Collector
{
    public class HotChocolateCollector : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public HotChocolateCollector(Tracer tracer)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
                name => new HotChocolateDiagnosticListener(name, tracer),
               listener => listener.Name.Contains("HotChocolate"),
               null);
            _diagnosticSourceSubscriber.Subscribe();
        }

        public void Dispose()
        {
            _diagnosticSourceSubscriber?.Dispose();
        }
    }
}