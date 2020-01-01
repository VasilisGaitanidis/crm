using CRM.Personal.GraphType.Filters;
using HotChocolate.Types;

namespace CRM.Personal.GraphType
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(x => x.GetPersons())
                .Name("persons")
                .Type<ListType<PersonType>>()
                .UseFiltering<PersonFilterType>();
        }
    }
}