using CRM.Protobuf.Contacts.V1;
using MediatR;

namespace CRM.Contact.Commands
{
    public class CreateContactCommand : IRequest<ContactDto>
    {
        public CreateContactCommand(CreateContactRequest contactRequest)
        {
            ContactRequest = contactRequest;

        }
        public CreateContactRequest ContactRequest { get; private set; }
    }
}
