using System;

namespace CRM.Personal.Domain
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        // public UserStatus UserStatus { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileName { get; set; }
        public string Fax { get; set; }
        public string LandLineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Website { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String ZipCode { get; set; }
        public String Country { get; set; }
    }
}