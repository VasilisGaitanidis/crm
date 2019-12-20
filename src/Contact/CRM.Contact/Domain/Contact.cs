using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Contact.Domain
{
    [Table("contact")]
    public class Contact
    {
        [Key]
        [Column("contact_id")]
        public Guid ContactId { get; set; }
        [Column("contact_type")]
        public ContactType ContactType { get; set; }
        [Column("first_name")]
        public String FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("middle_name")]
        public string MiddleName { get; set; }
        [Column("title")]
        public String Title { get; set; }
        [Column("company")]
        public String Company { get; set; }
        [Column("description")]
        public String Description { get; set; }
        [Column("photo")]
        public String Photo { get; set; }

        public ContactInformation ContactInfo { get; set; }

        public StreetAddress MailingAddress { get; set; }
    }

    public enum Gender
    {
        Mr = 0,
        Mrs = 1,
        Ms = 2
    }

    public enum ContactType
    {
        Lead = 0,
        Contact = 1,

    }
}
