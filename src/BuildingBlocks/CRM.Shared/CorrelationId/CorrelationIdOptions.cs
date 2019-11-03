namespace CRM.Shared.CorrelationId
{
    public class CorrelationIdOptions
    {
        private const string DefaultHeader = "X-Correlation-ID";
        public string Header { get; set; } = DefaultHeader;
        public bool IncludeInResponse { get; set; } = true;
        public bool UpdateTraceIdentifier { get; set; } = true;
        public bool UseGuidForCorrelationId { get; set; } = false;
    }
}