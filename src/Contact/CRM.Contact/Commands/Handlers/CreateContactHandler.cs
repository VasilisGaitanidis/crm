using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using CRM.Shared.ValidationModel;
using CRM.Protobuf.Contacts.V1;
using CRM.Contact.Domain;
using CRM.Shared.Repository;
using CRM.Contact.Extensions;
using Microsoft.Extensions.Logging;
using MassTransit;
using CRM.IntegrationEvents;
using CRM.Shared.CorrelationId;

namespace CRM.Contact.Commands.Handlers
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, CreateContactResponse>
    {
        private readonly IValidator<CreateContactRequest> _validator;
        private readonly ILogger<CreateContactHandler> _logger;
        private readonly IBusControl _bus;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CreateContactHandler(IValidator<CreateContactRequest> vadiator,
            ILogger<CreateContactHandler> logger, IBusControl bus, 
            ICorrelationContextAccessor correlationContextAccessor)
        {
            _validator = vadiator;
            _logger = logger;
            _bus = bus;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<CreateContactResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateContactHandler - handle");

            await _validator.HandleValidation(request.ContactRequest);
            var requestedContact = request.ContactRequest;
            var contact = new Domain.Contact(ContactType.Contact,
                requestedContact.FirstName,
                requestedContact.LastName,
                requestedContact.Title,
                requestedContact.Company,
                requestedContact.Description,
                null);

            // var contactId = await _uow.Connection.InsertAsync<Guid, Domain.Contact>(contact);

            await _bus.Publish<ContactCreated>(new
            {
                FirstName = "sss",
                CorrelationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId
            });

            return new CreateContactResponse
            {
                Contact = contact.ToContactProtobuf()
            };
        }
    }
}
