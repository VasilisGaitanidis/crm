using HotChocolate.Types;

namespace CRM.Personal.GraphType
{
    public sealed class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(t => t.CreateNewPerson(default))
                .Type<NonNullType<PersonType>>()
                .Argument("personInput", a => a.Type<NonNullType<PersonInputType>>());
        }
    }
}