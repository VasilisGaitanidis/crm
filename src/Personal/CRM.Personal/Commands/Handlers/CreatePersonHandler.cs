using System.Threading;
using System.Threading.Tasks;
using CRM.Personal.Extensions;
using CRM.Protobuf.Person.V1;
using CRM.Protobuf.Personal.V1;
using CRM.Shared.ValidationModel;
using FluentValidation;
using MediatR;

namespace CRM.Personal.Commands.Handlers
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDto>
    {
        private readonly IValidator<CreatePersonRequest> _validator;
        private readonly PersonalContext _context;

        public CreatePersonHandler(IValidator<CreatePersonRequest> vadiator, PersonalContext context)
        {
            _validator = vadiator;
            _context = context;
        }

        public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
             await _validator.HandleValidation(request.PersonRequest);
            var person = request.PersonRequest.ToPerson();
            
            var result = await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();

            return result.Entity.ToPersonProtobuf();
        }
    }
}