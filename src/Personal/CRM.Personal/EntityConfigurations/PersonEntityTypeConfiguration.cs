using CRM.Personal.Domain;
using CRM.Protobuf.Commons.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRM.Personal.EntityConfigurations
{
    public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("person", PersonalContext.DEFAULT_SCHEMA);
            builder.HasKey(x => x.PersonId);

            builder.Property(x=>x.PersonId).HasColumnName("person_id");
            builder.Property(x=>x.FirstName)
                .HasColumnName("first_name")
                .IsRequired();
            builder.Property(x=>x.LastName)
                .HasColumnName("last_name")
                .IsRequired();
            builder.Property(x=>x.Alias).HasColumnName("alias");
            builder.Property(x=>x.UserStatus)
                .HasColumnName("user_status")
                .HasConversion(new EnumToStringConverter<UserStatus>())
                .IsRequired();
            builder.Property(x=>x.UserName).HasColumnName("user_name");
            builder.Property(x=>x.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(x=>x.ProfileName).HasColumnName("profile_name");
            builder.Property(x => x.Street).HasColumnName("street");
            builder.Property(x => x.City).HasColumnName("city");
            builder.Property(x => x.State).HasColumnName("state");
            builder.Property(x => x.ZipCode).HasColumnName("zipcode");
            builder.Property(x => x.Country).HasColumnName("country");

            builder.Property(x => x.Fax).HasColumnName("fax");
            builder.Property(x => x.LandLineNumber).HasColumnName("landline_number");
            builder.Property(x => x.MobileNumber).HasColumnName("mobile_number");
            builder.Property(x => x.Website).HasColumnName("website");
        }
    }
}