using System;
using CRM.Protobuf.Contacts.V1;

namespace CRM.Contact.Extensions
{
    public static class ConvertProtobufToDomain
    {
        public static Domain.Contact ToContact(this CreateContactRequest contactRequest)
        {
            return new Domain.Contact 
            {
                ContactId = Guid.NewGuid(),
                ContactType = Domain.ContactType.Contact,
                FirstName = contactRequest.FirstName,
                LastName = contactRequest.LastName,
                Title = contactRequest.Title,
                Company = contactRequest.Company,
                Description = contactRequest.Description
            };
        }
    }
}