using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Extensions;
using CRM.Protobuf.Contacts.V1;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.Queries.Handlers
{
    public class FindAllContactsHandler : IRequestHandler<FindAllContactsQuery, ListContactsResponse>
    {
        private readonly ILogger<FindAllContactsHandler> _logger;
        private readonly ContactContext _context;

        public FindAllContactsHandler(ContactContext context, ILogger<FindAllContactsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ListContactsResponse> Handle(FindAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _context.Contacts
                .AsNoTracking()
                .ToListAsync();
                
            var response = new ListContactsResponse();
            _logger.LogInformation("Start query all contacts");
            response.Contacts.AddRange(contacts.Select(c => c.ToContactProtobuf()));
            return response;
        }
    }
}
