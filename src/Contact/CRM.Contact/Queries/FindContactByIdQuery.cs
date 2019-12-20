using System;
using CRM.Protobuf.Contacts.V1;
using MediatR;

namespace CRM.Contact.Queries
{
    public class FindContactByIdQuery : IRequest<ContactDto>
    {
        public FindContactByIdQuery(Guid contactId)
        {
            ContactId = contactId;

        }
        public Guid ContactId { get; private set; }

    }
}
