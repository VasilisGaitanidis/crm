using System.Threading.Tasks;
using CRM.Contact.Commands;
using CRM.Protobuf.Contacts.V1;
using MediatR;

namespace CRM.Contact.GraphType
{
    public class Mutation
    {
        private readonly IMediator _mediator;
        public Mutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ContactDto> CreateNewContact(CreateContactRequest contactInput)
        {
            return await _mediator.Send(new CreateContactCommand(contactInput));
        }
    }
}