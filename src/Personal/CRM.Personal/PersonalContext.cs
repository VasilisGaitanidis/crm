using CRM.Personal.Domain;
using CRM.Personal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CRM.Personal
{
    public class PersonalContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "public";

        public DbSet<Person> Persons { get; set; }

        public PersonalContext(DbContextOptions<PersonalContext> options) : base(options)
        {
            System.Diagnostics.Debug.WriteLine("PersonalContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonEntityTypeConfiguration());
        }
    }
}