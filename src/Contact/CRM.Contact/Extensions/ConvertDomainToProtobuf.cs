namespace CRM.Contact.Extensions
{
    public static class ConvertDomainToProtobuf
    {
        public static Protobuf.Contacts.V1.Contact ToContactProtobuf(this Domain.Contact contact)
        {
            return new Protobuf.Contacts.V1.Contact
            {
                Id = contact.ContactId.ToString(),
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
                Description = contact.Description,
                Title = contact.Title,
                // Address = new Protobuf.Contacts.V1.Address
                // {
                //     City = contact.MailingCity,
                //     Country = contact.MailingCountry,
                //     State = contact.MailingState,
                //     Street = contact.MailingStreet,
                //     Zipcode = contact.MailingZipCode
                // },
                // ContactInfo = new Protobuf.Contacts.V1.ContactInformation
                // {
                //     Email = contact.Email,
                //     HomePhone = contact.HomePhone,
                //     Mobile = contact.Mobile,
                //     WorkPhone = contact.WorkPhone
                // }
            };
        }
    }
}
