using System.Linq;
using CRM.Protobuf.Person.V1;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Personal.Queries.Handlers
{
    public class GetPersonsHandler : RequestHandler<GetPersonsQuery, IQueryable<PersonDto>>
    {
        private readonly ILogger<GetPersonsHandler> _logger;
        private readonly PersonalContext _context;

        public GetPersonsHandler(ILogger<GetPersonsHandler> logger, PersonalContext context)
        {
            _logger = logger;
            _context = context;
        }

        protected override IQueryable<PersonDto> Handle(GetPersonsQuery request)
        {
            _logger.LogInformation("GetPersonsHandler - Get persons.");

            return _context.Persons
                .AsNoTracking()
                .Select(x => new PersonDto
                {
                    PersonId = x.PersonId.ToString(),
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Alias = x.Alias,
                    Email = x.Email,
                    ProfileName = x.ProfileName,
                    UserName = x.UserName,
                    UserStatus = x.UserStatus
                })
                .AsQueryable();
        }
    }
}