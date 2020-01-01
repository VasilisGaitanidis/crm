using System.Linq;
using CRM.Protobuf.Person.V1;
using MediatR;

namespace CRM.Personal.Queries
{
    public class GetPersonsQuery : IRequest<IQueryable<PersonDto>>
    {
        
    }
}