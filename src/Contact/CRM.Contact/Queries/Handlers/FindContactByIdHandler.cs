using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Extensions;
using CRM.Protobuf.Contacts.V1;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Contact.Queries.Handlers
{
    public class FindContactByIdHandler : IRequestHandler<FindContactByIdQuery, ContactDto>
    {

        private readonly ContactContext _context;

        public FindContactByIdHandler(ContactContext context)
        {
            _context = context;
        }

        public async Task<ContactDto> Handle(FindContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _context.Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ContactId == request.ContactId);

            return contact.ToContactProtobuf();
        }
    }
}
