using System.Threading.Tasks;
using CRM.Protobuf.Commons.V1;
using CRM.Protobuf.Contacts.V1;
using Google.Protobuf.WellKnownTypes;

namespace CRM.Contact.Services
{
    public class ContactService : ContactApi.ContactApiBase
    {
        public override Task<PongReply> Ping(Empty request, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(new PongReply
            {
                Message = "Contact service pong"
            });
        }
    }
}
