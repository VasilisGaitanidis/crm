using HotChocolate.Types;

namespace CRM.Contact.GraphType
{
    public class ContactType: ObjectType<Domain.Contact>
    {
        protected override void Configure(IObjectTypeDescriptor<Domain.Contact> descriptor)
        {
            descriptor.Field(t => t.ContactInfo).Type<ContactInformationType>();
            descriptor.Field(t => t.MailingAddress).Type<AddressType>();
        }
    }
}