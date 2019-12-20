using CRM.Protobuf.Contacts.V1;
using HotChocolate.Types;

namespace CRM.Contact.GraphType
{
    public class ContactType: ObjectType<ContactDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ContactDto> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(null)).Ignore();
            
            descriptor.Field(t => t.ContactInfo).Type<ContactInformationType>();
            descriptor.Field(t => t.MailingAddress).Type<AddressType>();
        }
    }
}