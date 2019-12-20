using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.Protobuf.Contacts.V1;
using MediatR;

namespace CRM.Contact.GraphType
{
    public class Query
    {
        private readonly IMediator _mediator;
        public Query(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IEnumerable<ContactDto>> GetContacts()
        {
            return await _mediator.Send(new FindAllContactsQuery());
        }

        public async Task<ContactDto> GetContactById(Guid contactId)
        {
            return await _mediator.Send(new FindContactByIdQuery(contactId));
        }
    }
}