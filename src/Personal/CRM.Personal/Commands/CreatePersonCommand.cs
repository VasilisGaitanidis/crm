using CRM.Protobuf.Person.V1;
using CRM.Protobuf.Personal.V1;
using MediatR;

namespace CRM.Personal.Commands
{
    public class CreatePersonCommand : IRequest<PersonDto>
    {
        public CreatePersonCommand(CreatePersonRequest personRequest)
        {
            this.PersonRequest = personRequest;

        }
        public CreatePersonRequest PersonRequest { get; private set; }
    }
}