using CRM.Personal.Domain;
using CRM.Protobuf.Commons.V1;
using CRM.Protobuf.Person.V1;

namespace CRM.Personal.Extensions
{
    public static class ConvertDomainToProtobuf
    {
        public static PersonDto ToPersonProtobuf(this Person person)
        {
            return new PersonDto
            {
                PersonId = person.PersonId.ToString(),
                FirstName = person.FirstName,
                LastName = person.LastName,
                Alias = person.Alias,
                Email = person.Email,
                ProfileName = person.ProfileName,
                UserName = person.UserName,
                UserStatus = person.UserStatus,
                Address = new StreetAddressDto
                {
                    City = person.City,
                    Country = person.Country,
                    State = person.State,
                    Street = person.Street
                },
                PersonInfo = new PersonInfoDto
                {
                    Fax = person.Fax,
                    LandLineNumber = person.LandLineNumber,
                    MobileNumber = person.MobileNumber,
                    Website = person.Website
                }
            };
        }
    }
}