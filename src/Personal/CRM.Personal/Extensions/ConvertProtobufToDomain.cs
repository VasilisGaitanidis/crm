using System;
using CRM.Personal.Domain;
using CRM.Protobuf.Personal.V1;

namespace CRM.Personal.Extensions
{
    public static class ConvertProtobufToDomain
    {
        public static Person ToPerson(this CreatePersonRequest personRequest)
        {
            return new Person
            {
                PersonId = Guid.NewGuid(),
                FirstName = personRequest.FirstName,
                LastName = personRequest.LastName,
                Alias = personRequest.Alias,
                UserStatus = personRequest.UserStatus,
                UserName = personRequest.UserName,
                Email = personRequest.Email,
                ProfileName = personRequest.ProfileName
            };
        }
    }
}