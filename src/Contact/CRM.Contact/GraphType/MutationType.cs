using HotChocolate.Types;

namespace CRM.Contact.GraphType
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(t => t.CreateNewContact(default))
                .Type<NonNullType<ContactType>>()
                .Argument("contactInput", a => a.Type<NonNullType<ContactInputType>>());
        }
    }
}