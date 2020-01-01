using System.Threading.Tasks;
using CRM.Personal.Queries;
using CRM.Protobuf.Commons.V1;
using CRM.Protobuf.Personal.V1;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace CRM.Personal.Services
{
    public class PersonalService : PersonalApi.PersonalApiBase
    {
        private readonly IMediator _mediator;

        public PersonalService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<PongReply> Ping(Empty request, Grpc.Core.ServerCallContext context)
        {
            return await _mediator.Send(new PingQuery());
        }
    }
}