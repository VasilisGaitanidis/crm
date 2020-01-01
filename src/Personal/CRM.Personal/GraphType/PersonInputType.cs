using CRM.Protobuf.Personal.V1;
using HotChocolate.Types;

namespace CRM.Personal.GraphType
{
    public class PersonInputType : InputObjectType<CreatePersonRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePersonRequest> descriptor)
        {
            descriptor.Name("PersonInput");
        }
    }
}