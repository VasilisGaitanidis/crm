using CRM.Protobuf.Contacts.V1;
using HotChocolate.Types;

namespace CRM.Contact.GraphType
{
    public class ContactInformationType : ObjectType<ContactInfoDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ContactInfoDto> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(null)).Ignore();
        }
    }
}