using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using CRM.Shared.ValidationModel;
using CRM.Protobuf.Contacts.V1;
using CRM.Contact.Extensions;
using Microsoft.Extensions.Logging;
using MassTransit;
using CRM.Shared.CorrelationId;
using CRM.Contact.IntegrationEvents;

namespace CRM.Contact.Commands.Handlers
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, ContactDto>
    {
        private readonly IValidator<CreateContactRequest> _validator;
        private readonly ILogger<CreateContactHandler> _logger;
        private readonly IBusControl _bus;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        private readonly ContactContext _context;

        public CreateContactHandler(IValidator<CreateContactRequest> vadiator,
             ILogger<CreateContactHandler> logger, 
            IBusControl bus, 
            ICorrelationContextAccessor correlationContextAccessor,
            ContactContext context)
        {
            _validator = vadiator;
            _logger = logger;
            _bus = bus;
            _correlationContextAccessor = correlationContextAccessor;
            _context = context;
        }

        public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateContactHandler - handle");

            await _validator.HandleValidation(request.ContactRequest);

            var contact = request.ContactRequest.ToContact();
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync(cancellationToken);
            
            await _bus.Publish<ContactCreated>(new
            {
                contact.FirstName,
                _correlationContextAccessor?.CorrelationContext?.CorrelationId
            });

            return contact.ToContactProtobuf();
        }
    }
}
