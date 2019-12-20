using HotChocolate.Types;

namespace CRM.Contact.GraphType
{
    public sealed class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(t => t.GetContacts()).Type<NonNullType<ListType<ContactType>>>();

            descriptor.Field(t => t.GetContactById(default))
                .Name("contactById")
                .Type<ContactType>()
                .Argument("contactId", x => x.Type<UuidType>());
        }
    }
}