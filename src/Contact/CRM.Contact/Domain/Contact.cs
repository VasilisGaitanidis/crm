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
        public Guid ContactId { get; private set; }
        [Column("contact_type")]
        public ContactType ContactType { get; private set; }
        [Column("first_name")]
        public String FirstName { get; private set; }
        [Column("last_name")]
        public string LastName { get; private set; }
        [Column("middle_name")]
        public string MiddleName { get; private set; }
        [Column("title")]
        public String Title { get; private set; }
        [Column("company")]
        public String Company { get; private set; }
        [Column("description")]
        public String Description { get; private set; }
        [Column("photo")]
        public String Photo { get; private set; }
        // [Column("email")]
        // public String Email { get; private set; }
        // [Column("mobile")]
        // public String Mobile { get; private set; }
        // [Column("work_phone")]
        // public String WorkPhone { get; private set; }
        // [Column("home_phone")]
        // public String HomePhone { get; private set; }

        public ContactInformation ContactInfo { get; set; }

        public StreetAddress MailingAddress { get; set; }
        // [Column("mailing_street")]
        // public String MailingStreet { get; private set; }
        // [Column("mailing_city")]
        // public String MailingCity { get; private set; }
        // [Column("mailing_state")]
        // public String MailingState { get; private set; }
        // [Column("mailing_zipcode")]
        // public String MailingZipCode { get; private set; }
        // [Column("mailing_country")]
        // public String MailingCountry { get; private set; }

        private Contact() { }
        public Contact(ContactType contactType, string firstName, string lastName,
            string title, string company, string description, string photo)
        {
            ContactType = contactType;
            FirstName = firstName;
            LastName = lastName;
            Title = title;
            Company = company;
            Description = description;
            Photo = photo;
        }

        // public void AddContactInfo(string email, string mobile, string workPhone, string homePhone)
        // {
        //     Email = email;
        //     Mobile = mobile;
        //     WorkPhone = workPhone;
        //     HomePhone = homePhone;
        // }

        // public void AddAddress(string street, string country, string city, string zipcode, string state)
        // {
        //     MailingStreet = street;
        //     MailingCountry = country;
        //     MailingCity = city;
        //     MailingZipCode = zipcode;
        //     MailingState = state;
        // }
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
