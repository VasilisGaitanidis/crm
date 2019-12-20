using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Extensions;
using CRM.Protobuf.Contacts.V1;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Contact.Queries.Handlers
{
    public class FindAllContactsHandler : IRequestHandler<FindAllContactsQuery, IEnumerable<ContactDto>>
    {
        private readonly ContactContext _context;

        public FindAllContactsHandler(ContactContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactDto>> Handle(FindAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _context.Contacts
               .AsNoTracking()
               .Select(x => x.ToContactProtobuf())
               .ToListAsync();

            return contacts;
        }
    }
}
