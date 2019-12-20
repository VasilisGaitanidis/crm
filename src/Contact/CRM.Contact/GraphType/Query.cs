using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRM.Contact.GraphType
{
    public class Query
    {
        private readonly ContactContext _context;

        public Query(ContactContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Domain.Contact>> GetContacts()
        {
             var contacts = await _context.Contacts
                .AsNoTracking()
                .ToListAsync();
                
            return contacts;
        }
    }
}