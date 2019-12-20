using CRM.Protobuf.Contacts.V1;

namespace CRM.Contact.Extensions
{
    public static class ConvertDomainToProtobuf
    {
        public static ContactDto ToContactProtobuf(this Domain.Contact contact)
        {
            return new ContactDto
            {
                Id = contact.ContactId.ToString(),
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
                Description = contact.Description,
                Title = contact.Title,
                MailingAddress = new StreetAddressDto
                {
                    City = contact.MailingAddress?.City,
                    Country = contact.MailingAddress?.Country,
                    State = contact.MailingAddress?.State,
                    Street = contact.MailingAddress?.Street,
                    Zipcode = contact.MailingAddress?.ZipCode
                },
                ContactInfo = new ContactInfoDto
                {
                    Email = contact.ContactInfo?.Email,
                    HomePhone = contact.ContactInfo?.HomePhone,
                    Mobile = contact.ContactInfo?.Mobile,
                    WorkPhone = contact.ContactInfo?.WorkPhone
                }
            };
        }
    }
}
