using CRM.Contact.Domain;
using CRM.Contact.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CRM.Contact
{
    public class ContactContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "public";

        public DbSet<Domain.Contact> Contacts { get; set; }

        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
            System.Diagnostics.Debug.WriteLine("ContactContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
        }
    }
}