using CRM.Protobuf.Person.V1;
using HotChocolate.Types.Filters;

namespace CRM.Personal.GraphType.Filters
{
    public class PersonFilterType : FilterInputType<PersonDto>
    {
        protected override void Configure(
            IFilterInputTypeDescriptor<PersonDto> descriptor)
        {
            descriptor
                .BindFieldsExplicitly()
                .Filter(t => t.FirstName)
                .AllowEquals().And()
                .AllowContains();
            descriptor
                .BindFieldsExplicitly()
                .Filter(t => t.LastName)
                .AllowEquals().And()
                .AllowContains();
            descriptor
                .BindFieldsExplicitly()
                .Filter(t => t.UserStatus)
                .AllowEquals();
        }
    }
}