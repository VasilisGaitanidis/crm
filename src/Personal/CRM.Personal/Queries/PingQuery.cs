using CRM.Protobuf.Commons.V1;
using MediatR;

namespace CRM.Personal.Queries
{
    public class PingQuery : IRequest<PongReply>
    {
        
    }
}