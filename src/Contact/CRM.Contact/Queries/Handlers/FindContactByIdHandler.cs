using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CRM.Contact.Queries.Handlers
{
    public class FindContactByIdHandler : IRequestHandler<FindContactByIdQuery, CRM.Protobuf.Contacts.V1.Contact>
    {
        
        public FindContactByIdHandler()
        {            
        }

        public async Task<Protobuf.Contacts.V1.Contact> Handle(FindContactByIdQuery request, CancellationToken cancellationToken)
        {
            // var contact = await _uow.Connection.GetAsync<Domain.Contact>(request.ContactId);
            // if (contact == null)
            // {
            //     throw new RpcException(new Status(StatusCode.NotFound, $"Contact with Id {request.ContactId} not found."));
            // }
            // return contact.ToContactProtobuf();
            return null;
        }
    }
}
