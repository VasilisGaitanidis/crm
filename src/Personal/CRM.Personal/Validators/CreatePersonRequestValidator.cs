using CRM.Protobuf.Personal.V1;
using FluentValidation;

namespace CRM.Personal.Validators
{
    public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
    {
        public CreatePersonRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The first name could not be null or empty");

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The last name could not be null or empty");
            
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("The email could not be null or empty");
        }
    }
}