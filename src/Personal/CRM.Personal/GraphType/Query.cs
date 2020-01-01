using System.Linq;
using System.Threading.Tasks;
using CRM.Personal.Queries;
using CRM.Protobuf.Person.V1;
using MediatR;

namespace CRM.Personal.GraphType
{
    public class Query
    {
        private readonly IMediator _mediator;

        public Query(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IQueryable<PersonDto>> GetPersons()
        {
            return await _mediator.Send(new GetPersonsQuery());
        }
    }
}