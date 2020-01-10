namespace CRM.Graph.Gateway.Options
{
    public class ServiceOptions
    {
        public ServiceConfig ContactService { get; set; }
        public ServiceConfig IdentityService { get; set; }
        public ServiceConfig CommunicationService { get; set; }
        public ServiceConfig PersonalService { get; set; }
    }

    public class ServiceConfig
    {
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string GrpcUrl { get; set; }
    }
}
